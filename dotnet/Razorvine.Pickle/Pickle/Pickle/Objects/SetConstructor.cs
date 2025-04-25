/* part of Pickle, by Irmen de Jong (irmen@razorvine.net) */

using System.Collections;
using System.Collections.Generic;

namespace Razorvine.Pickle.Objects
{

/// <summary>
/// This object constructor creates sets. (HashSet&lt;object&gt;)
/// </summary>
public class SetConstructor : IObjectConstructor {

	public object construct(object[] args) {
		// create a HashSet, args=arraylist of stuff to put in it
		var elements=(ArrayList)args[0];
		return new HashSet<object>(elements.ToArray());
	}
}

}
