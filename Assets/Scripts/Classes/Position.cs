using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Position : System.Object {
    public int x;
    public int y;

    public Position(int x, int y) {
        this.x = x;
        this.y = y;
    }

    public Position(Position p) {
        this.x = p.x;
        this.y = p.y;
    }

    public static bool operator !=(Position a, Position b) {
        return !(a == b);
    }

    public static bool operator == (Position a, Position b) {
        // If both are null, or both are same instance, return true.
        if (System.Object.ReferenceEquals(a, b)) {
            return true;
        }

        // If one is null, but not both, return false.
        if (((object)a == null) || ((object)b == null)) {
            return false;
        }

        // Return true if the fields match:
        return a.x == b.x && a.y == b.y;
    }

    public override bool Equals(System.Object obj) {
        // If parameter is null return false.
        if (obj == null) {
            return false;
        }

        // If parameter cannot be cast to Point return false.
        Position p = obj as Position;
        if ((System.Object)p == null) {
            return false;
        }

        Debug.Log("equal");

        // Return true if the fields match:
        return (x == p.x) && (y == p.y);
    }

    public bool Equals(Position p) {
        // If parameter is null return false:
        if ((object)p == null) {
            return false;
        }

        // Return true if the fields match:
        return (x == p.x) && (y == p.y);
    }

    public override int GetHashCode() {
        return x ^ y;
    }
}