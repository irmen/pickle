package net.razorvine.pickle;

/**
 * Exception thrown when something goes wrong with pickling or unpickling.
 *
 * @author Irmen de Jong (irmen@razorvine.net)
 */
public class PickleException extends RuntimeException {
	private static final long serialVersionUID = -5870448664938735316L;

	public PickleException(String message, Throwable cause) {
		super(message, cause);
	}

	public PickleException(String message) {
		super(message);
	}
}
