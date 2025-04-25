/* part of Pickle, by Irmen de Jong (irmen@razorvine.net) */

// ReSharper disable UnusedMember.Global
namespace Razorvine.Pickle.Objects
{

/// <summary>
/// This object constructor uses reflection to create instances of the string type.
/// AnyClassConstructor cannot be used because string doesn't have the appropriate constructors.
///	see http://stackoverflow.com/questions/2092530/how-do-i-use-activator-createinstance-with-strings
/// </summary>
public class StringConstructor : IObjectConstructor
{
	public object construct(object[] args)
	{
		return args.Length switch
		{
			0 => "",
			1 when args[0] is string => args[0],
			_ => throw new PickleException("invalid string constructor arguments")
		};
	}
}

}
