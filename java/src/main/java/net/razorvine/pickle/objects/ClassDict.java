package net.razorvine.pickle.objects;

import java.util.HashMap;
import java.util.Map;

/**
 * A dictionary containing just the fields of the class.
 */
public class ClassDict extends HashMap<String, Object>
{
	private static final long serialVersionUID = 6157715596627049511L;
	private final String classname;

	public ClassDict(String modulename, String classname)
	{
		if(modulename==null)
			this.classname = classname;
		else
			this.classname = modulename+"."+classname;

		this.put("__class__", this.classname);
	}

	/**
	 * for the unpickler to restore state
	 */
	public void __setstate__(HashMap<String, Object> values) {
		this.clear();
		this.put("__class__", this.classname);
        this.putAll(values);
	}


	/**
	 * retrieve the (python) class name of the object that was pickled.
	 */
	public String getClassName() {
		return this.classname;
	}
}
