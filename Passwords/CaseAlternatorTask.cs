namespace Passwords;

public class CaseAlternatorTask
{
	//Тесты будут вызывать этот метод
	public static List<string> AlternateCharCases(string lowercaseWord)
	{
		var result = new List<string>();
		AlternateCharCases(lowercaseWord.ToCharArray(), 0, result);
		return result;
	}

	static void AlternateCharCases(char[] word, int startIndex, List<string> result)
	{
		if (TryAddCurrentCombination(word, startIndex, result))
		{
			return;
		}

		if (!char.IsLetter(word[startIndex]))
		{
			AlternateCharCases(word, startIndex + 1, result);
			return;
		}

		ProcessLetterVariants(word, startIndex, result);
	}

	static bool TryAddCurrentCombination(char[] word, int startIndex, List<string> result)
	{
		if (startIndex >= word.Length)
		{
			result.Add(new string(word));
			return true;
		}
		return false;
	}

	static void ProcessLetterVariants(char[] word, int startIndex, List<string> result)
	{
		var originalChar = word[startIndex];
		var lowerChar = char.ToLower(originalChar);
		var upperChar = char.ToUpper(originalChar);

		word[startIndex] = lowerChar;
		AlternateCharCases(word, startIndex + 1, result);

		if (upperChar != lowerChar)
		{
			word[startIndex] = upperChar;
			AlternateCharCases(word, startIndex + 1, result);
		}

		word[startIndex] = originalChar;
	}
}
