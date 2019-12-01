using System.Collections;
using System.Collections.Generic;

public class Coord
{
    public int x;
    public int y;

    public Coord(int x, int y) {
        this.x = x;
        this.y = y;
    }

    // Returns the Coord after applying the given direction to this Coord
    public Coord ApplyMovement(Direction dir, int distance) {
        switch (dir) {
            case (Direction.UP):
                return new Coord(x, y - distance);
            case (Direction.DOWN):
                return new Coord(x, y + distance);
            case (Direction.LEFT):
                return new Coord(x - distance, y);
            case (Direction.RIGHT):
                return new Coord(x + distance, y);
        }
        return this;
    }

    public override bool Equals(object o) {
        if (o is Coord) {
            return Equals((Coord)o);
        } else {
            return base.Equals(o);
        }
    }

    public bool Equals(Coord other) {
        if (other == null) {
            return false;
        }
        return this.x == other.x && this.y == other.y;
    }

    public static bool operator ==(Coord c1, Coord c2) {
        if (object.ReferenceEquals(c1, c2)) {
            return true;
        }
        if (object.ReferenceEquals(c1, null)) {
            return object.ReferenceEquals(c2, null);
        }
        return c1.Equals(c2);
	}

	public static bool operator !=(Coord c1, Coord c2) {
        if (object.ReferenceEquals(c1, c2)) {
            return false;
        }
        if (object.ReferenceEquals(c1, null)) {
            return !object.ReferenceEquals(c2, null);
        }
		return !c1.Equals(c2);
	}

    public override int GetHashCode() {
        return (x + "," + y).GetHashCode();

    }

    public override string ToString() {
		return "Coord (" + x + ", " + y + ")";
	}
}
