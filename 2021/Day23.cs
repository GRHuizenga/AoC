using Core;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2021
{
    public class Day23
    {
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
        public Day23() { }

        public int PartOne()
        {
            var hallway = Enumerable.Repeat(Amphipod.None, 7).ToArray();
            var rooms = new Stack<Amphipod>[4];
            rooms[0] = new Stack<Amphipod>(new[] { Amphipod.B, Amphipod.C });
            rooms[1] = new Stack<Amphipod>(new[] { Amphipod.A, Amphipod.D });
            rooms[2] = new Stack<Amphipod>(new[] { Amphipod.B, Amphipod.D });
            rooms[3] = new Stack<Amphipod>(new[] { Amphipod.C, Amphipod.A });
            
            return Solve(hallway, rooms, 2);
        }

        /// <summary>
        ///     #############
        ///     #..X.X.X.X..#
        ///     ###C#D#D#A###
        ///       #D#C#B#A#
        ///       #D#B#A#C#
        ///       #B#A#B#C#
        ///       #########
        /// </summary>
        public int PartTwo()
        {
            var hallway = Enumerable.Repeat(Amphipod.None, 7).ToArray();
            var rooms = new Stack<Amphipod>[4];
            rooms[0] = new Stack<Amphipod>(new[] { Amphipod.B, Amphipod.D, Amphipod.D, Amphipod.C });
            rooms[1] = new Stack<Amphipod>(new[] { Amphipod.A, Amphipod.B, Amphipod.C, Amphipod.D });
            rooms[2] = new Stack<Amphipod>(new[] { Amphipod.B, Amphipod.A, Amphipod.B, Amphipod.D });
            rooms[3] = new Stack<Amphipod>(new[] { Amphipod.C, Amphipod.C, Amphipod.A, Amphipod.A });

            return Solve(hallway, rooms, 4);
        }

        private Dictionary<(string, string), int> MemoizationCache = new();

        private int Solve(Amphipod[] hallway, Stack<Amphipod>[] rooms, int roomSize)
        {
            var hallwayStringRep = string.Join(string.Empty, hallway);
            var roomsStringRep = string.Join(string.Empty, rooms.Select(room => string.Join(string.Empty, Enumerable.Repeat(Amphipod.None, roomSize - room.Count).Concat(room))));

            if (MemoizationCache.TryGetValue((hallwayStringRep, roomsStringRep), out var cachedCost)) return cachedCost;

            if (IsFinalState(rooms, roomSize)) return 0;

            var best = int.MaxValue;

            foreach (var (cost, state) in Moves(hallway, rooms, roomSize).ToList())
            {
                var nCost = cost + Solve(state.hallway.ToArray(), state.rooms.ToArray(), roomSize);
                if (nCost > 0 && nCost < best) best = nCost;
            }

            MemoizationCache.Add((hallwayStringRep, roomsStringRep), best);

            return best;
        }

        private IEnumerable<(int cost, (Amphipod[] hallway, Stack<Amphipod>[] rooms))> Moves(Amphipod[] hallway, Stack<Amphipod>[] rooms, int roomSize)
        {
            foreach (var move in MoveToRoom(hallway, rooms, roomSize)) yield return move;
            foreach (var move in MoveToHallway(hallway, rooms, roomSize)) yield return move;
        }

        /// <summary>
        /// from hallway to room:
        ///     1. check if destination room is empty or occupied by the same kind of amphipod
        ///     2. check if path through the hallway is clear
        ///     3. calculate the cost of moving
        /// </summary>
        private IEnumerable<(int cost, (Amphipod[] hallway, Stack<Amphipod>[] rooms))> MoveToRoom(Amphipod[] hallway, Stack<Amphipod>[] rooms, int roomSize)
        {
            // only consider non-empty hallway spots
            foreach (var (amphipod, hallwaySpot) in hallway.Select((amphipod, index) => (amphipod, index)).Where(h => h.amphipod != Amphipod.None).ToList())
            {
                // get its destination room and see if it does not contain another type of amphipod
                var room = rooms[(int)amphipod];
                if (room.Any(pod => pod != amphipod)) continue;

                // calulate the cost for moving from the hallway spot to the room
                var cost = MoveCost(rooms, hallway, (int)amphipod, hallwaySpot, roomSize, true);

                // if its impossible to make the move, continue
                if (cost == int.MaxValue) continue;

                // return cost + new state
                var newHallway = new List<Amphipod>(hallway).ToArray();
                newHallway[hallwaySpot] = Amphipod.None;
                
                // yeah...it's a stack...reverse it first...
                var newRooms = rooms.Select(r => new Stack<Amphipod>(r.Reverse())).ToArray();
                newRooms[(int)amphipod].Push(amphipod);

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
        private IEnumerable<(int cost, (Amphipod[] hallway, Stack<Amphipod>[] rooms))> MoveToHallway(Amphipod[] hallway, Stack<Amphipod>[] rooms, int roomSize)
        {
            // only consider rooms which do not contain only amphipods for whom this is the destination room
            foreach (var (room, roomSpot) in rooms.Select((room, index) => (room, index)).ToList())
            {
                if (room.Count == 0 || room.All(amphipod => (int)amphipod == roomSpot)) continue;

                // for each destination hallway spot
                foreach (var hallwaySpot in Enumerable.Range(0, 7))
                {
                    // calculate the cost of moving there
                    var cost = MoveCost(rooms, hallway, roomSpot, hallwaySpot, roomSize);

                    // if we cannot move there, continue
                    if (cost == int.MaxValue) continue;

                    // return cost and new state
                    var newHallway = new List<Amphipod>(hallway).ToArray();
                    newHallway[hallwaySpot] = room.Peek();
                    // well, finding that missing reverse took me long enough...
                    var newRooms = rooms.Select(r => new Stack<Amphipod>(r.Reverse())).ToArray();
                    newRooms[roomSpot].Pop();

                    yield return (cost, (newHallway, newRooms));
                }
            }
        }

        private int MoveCost(Stack<Amphipod>[] rooms, Amphipod[] hallway, int roomSpot, int hallwaySpot, int roomSize, bool toRoom = false)
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

            var amphipod = toRoom ? hallway[hallwaySpot] : rooms[roomSpot].Peek();

            return (int)Math.Pow(10, (int)amphipod) * (Steps[roomSpot][hallwaySpot] + (roomSize - rooms[roomSpot].Count + (!toRoom ? 1 : 0)));
        }

        private bool IsFinalState(Stack<Amphipod>[] rooms, int roomSize)
        {
            if (Enumerable.Range(0, 4).Any(room => rooms[room].Count != roomSize || rooms[room].Any(amphipod => (int)amphipod != room))) return false;
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
