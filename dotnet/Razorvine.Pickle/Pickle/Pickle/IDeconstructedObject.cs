/* part of Pickle, by Irmen de Jong (irmen@razorvine.net) */

using System.IO;
// ReSharper disable InconsistentNaming

namespace Razorvine.Pickle {

	/// <summary>
	/// Interface for deconstructed objects used by the pickler, to pickle custom classes. 
	/// </summary>
	public interface IDeconstructedObject {
		string get_module();
		string get_name();
		object[] get_values();
		bool has_value();
	}

}
