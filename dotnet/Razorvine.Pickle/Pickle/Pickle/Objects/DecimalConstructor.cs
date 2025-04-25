/* part of Pickle, by Irmen de Jong (irmen@razorvine.net) */

using System.Globalization;

namespace Razorvine.Pickle.Objects
{

/// <summary>
/// This object constructor uses reflection to create instances of the decimal type.
/// (AnyClassConstructor cannot be used because decimal doesn't have the appropriate constructors).
/// </summary>
public class DecimalConstructor : IObjectConstructor
{
	public object construct(object[] args)
	{
		if(args.Length==1 && args[0] is string)
		{
			string stringArg = (string)args[0];
			if (stringArg.ToLowerInvariant()=="nan")
			{
				// special case Decimal("NaN") which is not supported in .NET, return this as double.NaN
				return double.NaN;
			}
			return decimal.Parse(stringArg, 
				NumberStyles.AllowLeadingSign | NumberStyles.AllowDecimalPoint | NumberStyles.AllowExponent, 
				CultureInfo.InvariantCulture);
		}

		throw new PickleException("invalid arguments for decimal constructor");
	}
}

}
