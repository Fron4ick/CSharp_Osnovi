namespace Pluralize;

public static class PluralizeTask
{
	public static string PluralizeRubles(int count)
	{
		var lastTwo = count % 100;
		var lastOne = count % 10;

		if (lastTwo >= 11 && lastTwo <= 14)
			return "рублей";

		return lastOne switch
		{
			1 => "рубль",
			2 or 3 or 4 => "рубля",
			_ => "рублей"
		};
	}
}