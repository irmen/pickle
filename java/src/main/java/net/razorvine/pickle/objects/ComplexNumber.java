package net.razorvine.pickle.objects;

import java.io.Serializable;

/**
 * An immutable Complex Number class.
 *
 * @author Irmen de Jong (irmen@razorvine.net)
 */
public class ComplexNumber implements Serializable {
	private static final long serialVersionUID = 4668080260997226513L;
	private final double r;	// real
	private final double i;	// imaginary

	public ComplexNumber(double rr, double ii) {
		r = rr;
		i = ii;
	}

	public ComplexNumber(Double rr, Double ii) {
		r = rr;
		i = ii;
	}

	public String toString() {
		StringBuilder sb = new StringBuilder().append(r);
		if (i >= 0)
			sb.append('+');
		return sb.append(i).append('i').toString();
	}

	public double getReal() {
		return r;
	}

	public double getImaginary() {
		return i;
	}

	public double magnitude() {
		return Math.sqrt(r * r + i * i);
	}

	public ComplexNumber add(ComplexNumber other) {
		return add(this, other);
	}

	public static ComplexNumber add(ComplexNumber c1, ComplexNumber c2) {
		return new ComplexNumber(c1.r + c2.r, c1.i + c2.i);
	}

	public ComplexNumber subtract(ComplexNumber other) {
		return subtract(this, other);
	}

	public static ComplexNumber subtract(ComplexNumber c1, ComplexNumber c2) {
		return new ComplexNumber(c1.r - c2.r, c1.i - c2.i);
	}

	public ComplexNumber multiply(ComplexNumber other) {
		return multiply(this, other);
	}

	public static ComplexNumber multiply(ComplexNumber c1, ComplexNumber c2) {
		return new ComplexNumber(c1.r * c2.r - c1.i * c2.i, c1.r * c2.i + c1.i * c2.r);
	}

	public static ComplexNumber divide(ComplexNumber c1, ComplexNumber c2) {
		return new ComplexNumber((c1.r * c2.r + c1.i * c2.i) / (c2.r * c2.r + c2.i * c2.i), (c1.i * c2.r - c1.r * c2.i)
				/ (c2.r * c2.r + c2.i * c2.i));
	}

	public boolean equals(Object o) {
		if (!(o instanceof ComplexNumber))
			return false;
		ComplexNumber other = (ComplexNumber) o;
		return r == other.r && i == other.i;
	}

	public int hashCode() {
		return (Double.valueOf(r).hashCode()) ^ (Double.valueOf(i).hashCode());
	}
}
