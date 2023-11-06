/* part of Pickle, by Irmen de Jong (irmen@razorvine.net) */

using System.IO;
// ReSharper disable InconsistentNaming

namespace Razorvine.Pickle {

	/// <summary>
	/// Interface for deconstructed objects used by the pickler, to pickle custom classes. 
	/// </summary>
	public interface IObjectDeconstructor {
		/// <summary>
		/// Get the module of the class being pickled
		/// </summary>
		string get_module();
        /// <summary>
        /// Get the name of the class being pickled
        /// </summary>
        string get_name();
        /// <summary>
        /// Get the deconstructed values, which will be used as arguments for reconstructing
        /// </summary>
        object[] deconstruct(object obj);
	}

}
