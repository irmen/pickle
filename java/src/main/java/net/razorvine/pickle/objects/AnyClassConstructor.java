package net.razorvine.pickle.objects;

import net.razorvine.pickle.IObjectConstructor;
import net.razorvine.pickle.PickleException;

import java.lang.reflect.Constructor;
import java.math.BigDecimal;

/**
 * This object constructor uses reflection to create instances of any given class.
 *
 * @author Irmen de Jong (irmen@razorvine.net)
 */
public class AnyClassConstructor implements IObjectConstructor {

	private final Class<?> type;

	public AnyClassConstructor(Class<?> type) {
		this.type = type;
	}

	public Object construct(Object[] args) {
		try {
			Class<?>[] paramtypes = new Class<?>[args.length];
			for (int i = 0; i < args.length; ++i) {
				paramtypes[i] = args[i].getClass();
			}
			Constructor<?> cons = type.getConstructor(paramtypes);

			// special case BigDecimal("NaN") which is not supported in Java, return this as Double.NaN
			if(type == BigDecimal.class && args.length==1) {
				String nan = (String) args[0];
				if(nan.equalsIgnoreCase("nan"))
					return Double.NaN;
			}

			return cons.newInstance(args);
		} catch (Exception x) {
			throw new PickleException("problem construction object: " + x);
		}
	}
}
