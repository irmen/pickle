/* part of Pickle, by Irmen de Jong (irmen@razorvine.net) */

using System.IO;
// ReSharper disable InconsistentNaming

namespace Razorvine.Pickle {

	/// <summary>
	/// Interface for object deconstructors used by the pickler, to pickle custom classes. 
	/// </summary>
	public interface IObjectDeconstructor {
		string get_module();
		string get_name();
		object get_value();
		bool has_value();
	}

}
