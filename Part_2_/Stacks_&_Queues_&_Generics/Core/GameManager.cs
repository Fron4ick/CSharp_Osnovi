using MagneticMaze.Core.Physics;
using MagneticMaze.Gameplay;
using MagneticMaze.Gameplay.Level;
using MagneticMaze.Rendering;
using MagneticMaze.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MagneticMaze.Core;

public enum GameState
{
    MainMenu,
    Playing,
    Paused,
    LevelComplete,
    Options
}

public class GameManager : Game
{
    private readonly GraphicsDeviceManager _graphics;
    private SpriteBatch? _spriteBatch;

    private BallPhysics? _ballPhysics;
    private MagnetControl? _magnetControl;
    private LevelGenerator? _levelGenerator;
    private RenderingManager? _rendering;
    private UIManager? _ui;

    private GameState _currentState;
    private LevelData _currentLevel;

    public GameManager()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
        Window.Title = "Magnetic Maze";
    }

    protected override void Initialize()
    {
        _ballPhysics = new BallPhysics(
            mass: 1.0f,
            magneticConstant: 10.0f,
            damping: 0.99f,
            maxSpeed: 5.0f,
            restitution: 0.8f);

        _magnetControl = new MagnetControl(
            sphereRadius: 5.0f,
            rotationSpeed: 2.0f);

        _levelGenerator = new LevelGenerator();
        _rendering = new RenderingManager(GraphicsDevice, Content);
        _ui = new UIManager(GraphicsDevice, Content);

        _currentState = GameState.MainMenu;

        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);
        _rendering?.LoadContent();
        _ui?.LoadContent();
    }

    protected override void Update(GameTime gameTime)
    {
        var dt = (float)gameTime.ElapsedGameTime.TotalSeconds;

        var keyboard = Keyboard.GetState();
        var mouse = Mouse.GetState();

        if (keyboard.IsKeyDown(Keys.Escape) && _currentState == GameState.Playing)
        {
            _currentState = GameState.Paused;
        }

        if (_ui != null)
        {
            var goalReached = _ballPhysics?.HasReachedGoal() ?? false;
            var polarity = _magnetControl?.GetPolarity() ?? 1f;

            _ui.Update(
                dt,
                _currentState,
                goalReached,
                polarity,
                out var newState,
                out var exitRequested);

            if (exitRequested)
                Exit();

            if (newState != _currentState)
                SwitchState(newState);
        }

        if (_currentState == GameState.Playing &&
            _ballPhysics != null &&
            _magnetControl != null &&
            _rendering != null)
        {
            _magnetControl.Update(dt, keyboard);

            _ballPhysics.Update(
                dt,
                _magnetControl.GetMagnetPosition(),
                _magnetControl.GetPolarity());

            if (_ballPhysics.HasReachedGoal())
            {
                _currentState = GameState.LevelComplete;
            }

            _rendering.SetSceneData(
                _ballPhysics.GetBallPosition(),
                Quaternion.Identity,
                _magnetControl.GetOuterRingTransform(),
                _magnetControl.GetInnerRingTransform(),
                _magnetControl.GetMagnetTransform(),
                _currentLevel.Walls,
                _currentLevel.GoalPosition);
        }

        _rendering?.UpdateCamera(dt, keyboard, mouse);

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        _rendering?.Draw();
        _ui?.Draw(_currentState);

        base.Draw(gameTime);
    }

    private void SwitchState(GameState newState)
    {
        if (_magnetControl == null || _levelGenerator == null || _ballPhysics == null)
        {
            _currentState = newState;
            return;
        }

        switch (newState)
        {
            case GameState.Playing:
            {
                var parameters = new LevelParameters
                {
                    Seed = 42,
                    GridSizeX = 5,
                    GridSizeY = 5,
                    GridSizeZ = 5,
                    CellSize = 1.0f
                };

                _currentLevel = _levelGenerator.GenerateLevel(parameters);
                _ballPhysics.SetLevelData(
                    _currentLevel.Walls,
                    _currentLevel.StartPosition,
                    _currentLevel.GoalPosition);

                _magnetControl.Reset();
                break;
            }
            case GameState.MainMenu:
                break;
            case GameState.Paused:
                break;
            case GameState.LevelComplete:
                break;
            case GameState.Options:
                break;
        }

        _currentState = newState;
    }
}

