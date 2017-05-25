using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move {
    public Position start;
    public Position end;
    public Move(Position start, Position end) {
        this.start = start;
        this.end = end;
    }
}
