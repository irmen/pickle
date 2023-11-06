package net.razorvine.pickle;

/**
 * Interface for Object Deconstructors that are used by the pickler
 * to create instances of non-primitive or custom classes.
 *
 * @author Irmen de Jong (irmen@razorvine.net)
 */
public interface IObjectDeconstructor {
	/**
	 * Get the module of the class being pickled
	 */
	String getModule();
	/**
	 * Get the name of the class being pickled
	 */
	String getName();
	/**
	 * Deconstructs the arugment of an object. The given args will be used as parameters for the constructor during unpickling.
	 */
	Object[] deconstruct(Object obj) throws PickleException;
	
}
