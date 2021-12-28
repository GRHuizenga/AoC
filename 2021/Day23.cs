using Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2021
{
    public class Day23
    {
        private Amphipod[] Hallway;
        private (Amphipod spot1, Amphipod spot2)[] Rooms;

        // Steps from any hallway position to the entrance of a room
        private int[][] Steps = new int[4][]
        {
            new int[] { 2, 1, 1, 3, 5, 7, 8 }, // from/to room 0
            new int[] { 4, 3, 1, 1, 3, 5, 6 }, // from/to room 1
            new int[] { 6, 5, 3, 1, 1, 3, 4 }, // from/to room 2
            new int[] { 8, 7, 5, 3, 1, 1, 2 }, // from/to room 3
        };

        /// <summary>
        ///     #############       A: amphipod, step costs 1
        ///     #..X.X.X.X..#       B: amphipod, step costs 10
        ///     ###C#D#D#A###       C: amphipod, step costs 100
        ///       #B#A#B#C#         D: amphipod, step costs 1000
        ///       #########         X: illegal spot
        ///                         .: empty spot
        ///                         
        /// representation of state:
        ///     Hallway => Amphipod[7] (ignore illegal spots)      => init: [None, None, None, None, None, None, None]
        ///     Rooms   => (Amphipod spot1, Amphipod spot2)[]      => init: [(C, B), (D, A), (D, B), (A, C)]
        ///     
        /// destination state:
        ///     Hallway => Amphipod[7] (ignore illegal spots)      => init: [None, None, None, None, None, None, None]
        ///     Rooms   => (Amphipod spot1, Amphipod spot2)[]      => init: [(A, A), (B, B), (C, C), (D, D)]
        ///     
        /// rules:
        ///     - amphipods cannot stay on a spot marked X above
        ///     - once out of their room, they can only move into their destination room, but only if empty or only contains amphipods like themselves
        ///     - once out of their room in the hallway, they stay in that same spot until they can move into their destination room
        /// </summary>
        public Day23()
        {
            Hallway = Enumerable.Repeat(Amphipod.None, 7).ToArray();
            Rooms = new (Amphipod, Amphipod)[]
            {
                (Amphipod.C, Amphipod.B),
                (Amphipod.D, Amphipod.A),
                (Amphipod.D, Amphipod.B),
                (Amphipod.A, Amphipod.C),
            };
        }

        public int PartOne()
        {
            return Solve(Hallway, Rooms);
        }

        public int PartTwo()
        {
            return -1;
        }

        private int Solve(Amphipod[] hallway, (Amphipod, Amphipod)[] rooms)
        {
            if (IsFinalState(rooms)) return 0;

            var best = int.MaxValue;

            foreach (var (cost, state) in Moves(hallway, rooms).ToList())
            {
                var nCost = cost + Solve(state.hallway.ToArray(), state.rooms.ToArray());
                if (nCost > 0 && nCost < best) best = nCost;
            }

            return best;
        }

        private IEnumerable<(int cost, (Amphipod[] hallway, (Amphipod, Amphipod)[] rooms))> Moves(Amphipod[] hallway, (Amphipod, Amphipod)[] rooms)
        {
            foreach (var move in MoveToRoom(hallway, rooms)) yield return move;
            foreach (var move in MoveToHallway(hallway, rooms)) yield return move;
        }

        /// <summary>
        /// from hallway to room:
        ///     1. check if destination room is empty or occupied by the same kind of amphipod
        ///     2. check if path through the hallway is clear
        ///     3. calculate the cost of moving
        /// </summary>
        private IEnumerable<(int cost, (Amphipod[] hallway, (Amphipod, Amphipod)[] rooms))> MoveToRoom(Amphipod[] hallway, (Amphipod spot1, Amphipod spot2)[] rooms)
        {
            // only consider non-empty hallway spots
            foreach (var (amphipod, hallwaySpot) in hallway.Select((amphipod, index) => (amphipod, index)).Where(h => h.amphipod != Amphipod.None).ToList())
            {
                // get its destination room and see if it does not contain another type of amphipod
                var room = rooms[(int)amphipod];
                if (room.spot1 != Amphipod.None || (room.spot2 != Amphipod.None && room.spot2 != amphipod)) continue;

                // calulate the cost for moving from the hallway spot to the room
                var cost = MoveCost(rooms, hallway, (int)amphipod, hallwaySpot, true);

                // if its impossible to make the move, continue
                if (cost == int.MaxValue) continue;

                // return cost + new state
                var newHallway = new List<Amphipod>(hallway).ToArray();
                newHallway[hallwaySpot] = Amphipod.None;
                var newRooms = new List<(Amphipod, Amphipod)>(rooms).ToArray();
                newRooms[(int)amphipod] = room.spot2 == Amphipod.None ? (Amphipod.None, amphipod) : (amphipod, room.spot2);

                yield return (cost, (newHallway, newRooms));
            }
        }

        /// <summary>
        /// from room to hallway:
        ///     1. check if the room only contains amphipods for whom this room is the destination => skip it
        ///     2. check if the path to the hallway destination spot is free
        ///     3. calculate the cost of moving
        /// </summary>
        /// <returns></returns>
        private IEnumerable<(int cost, (Amphipod[] hallway, (Amphipod, Amphipod)[] rooms))> MoveToHallway(Amphipod[] hallway, (Amphipod spot1, Amphipod spot2)[] rooms)
        {
            // only consider rooms which do not contain only amphipods for whom this is the destination room
            foreach (var (room, roomSpot) in rooms.Select((room, index) => (room, index)).ToList())
            {
                if (room.spot1 == Amphipod.None && room.spot2 == Amphipod.None) continue;
                if (room.spot1 == Amphipod.None && (int)room.spot2 == roomSpot) continue;
                if ((int)room.spot1 == roomSpot && (int)room.spot2 == roomSpot) continue;

                // for each destination hallway spot
                foreach (var hallwaySpot in Enumerable.Range(0, 7))
                {
                    // calculate the cost of moving there
                    var cost = MoveCost(rooms, hallway, roomSpot, hallwaySpot);

                    // if we cannot move there, continue
                    if (cost == int.MaxValue) continue;

                    // return cost and new state
                    var newHallway = new List<Amphipod>(hallway).ToArray();
                    newHallway[hallwaySpot] = room.spot1 != Amphipod.None ? room.spot1 : room.spot2;
                    var newRooms = new List<(Amphipod, Amphipod)>(rooms).ToArray();
                    newRooms[roomSpot] = room.spot1 == Amphipod.None ? (Amphipod.None, Amphipod.None) : (Amphipod.None, room.spot2);

                    yield return (cost, (newHallway, newRooms));
                }
            }
        }

        private int MoveCost((Amphipod, Amphipod)[] rooms, Amphipod[] hallway, int roomSpot, int hallwaySpot, bool toRoom = false)
        {
            var start = 0;
            var end = 0;
            if (roomSpot + 1 < hallwaySpot)
            {
                start = roomSpot + 2;
                end = hallwaySpot + (toRoom ? -1 : 0);
            }
            else
            {
                start = hallwaySpot + (toRoom ? 1 : 0);
                end = roomSpot + 1;
            }

            if (Enumerable.Range(start, (end - start) + 1).Any(hallwayIndex => hallway[hallwayIndex] != Amphipod.None)) return int.MaxValue;

            var amphipod = toRoom ? hallway[hallwaySpot] : rooms[roomSpot].Item1 != Amphipod.None ? rooms[roomSpot].Item1 : rooms[roomSpot].Item2;

            var steps = 0;
            if (toRoom)
            {
                steps = rooms[roomSpot].Item2 == Amphipod.None ? 2 : 1;
            }
            else
            {
                steps = rooms[roomSpot].Item1 == Amphipod.None ? 2 : 1;
            }

            var cost = (int)Math.Pow(10, (int)amphipod) * (Steps[roomSpot][hallwaySpot] + steps);
            return cost;
        }

        private bool IsFinalState((Amphipod, Amphipod)[] rooms)
        {
            if (Enumerable.Range(0, 4).Any(roomSpot => (int)rooms[roomSpot].Item1 != roomSpot || (int)rooms[roomSpot].Item2 != roomSpot)) return false;
            return true;
        }
    }

    public enum Amphipod
    {
        None = -1,
        A = 0,
        B = 1,
        C = 2,
        D = 3,
    }
}
