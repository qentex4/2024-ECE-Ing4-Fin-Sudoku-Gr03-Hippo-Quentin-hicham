﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace Sudoku.ResolutionTechniquesHumaines;

partial class Solver
{
	private bool NakedQuadruple()
	{
		for (int i = 0; i < 9; i++)
		{
			if (NakedTuple_Find(Puzzle.BlocksI[i], 4)
				|| NakedTuple_Find(Puzzle.RowsI[i], 4)
				|| NakedTuple_Find(Puzzle.ColumnsI[i], 4))
			{
				return true;
			}
		}
		return false;
	}

	private bool NakedTriple()
	{
		for (int i = 0; i < 9; i++)
		{
			if (NakedTuple_Find(Puzzle.BlocksI[i], 3)
				|| NakedTuple_Find(Puzzle.RowsI[i], 3)
				|| NakedTuple_Find(Puzzle.ColumnsI[i], 3))
			{
				return true;
			}
		}
		return false;
	}

	private bool NakedPair()
	{
		for (int i = 0; i < 9; i++)
		{
			if (NakedTuple_Find(Puzzle.BlocksI[i], 2)
				|| NakedTuple_Find(Puzzle.RowsI[i], 2)
				|| NakedTuple_Find(Puzzle.ColumnsI[i], 2))
			{
				return true;
			}
		}
		return false;
	}

	private bool NakedTuple_Find(Region region, int amount)
	{
		return NakedTuple_Recurse(region, amount, 0, new Cell[amount], new int[amount]);
	}
	private bool NakedTuple_Recurse(Region region, int amount, int loop, Cell[] cells, int[] indexes)
	{
		if (loop == amount)
		{
			IEnumerable<int> combo = cells.Select(c => (IEnumerable<int>)c.CandI).UniteAll();

			if (combo.Count() == amount)
			{
				if (Cell.ChangeCandidates(indexes.Select(i => region[i].VisibleCells).IntersectAll(), combo))
				{
					LogAction(TechniqueFormat("Naked " + TupleStr[amount],
						"{0}: {1}",
						Utils.PrintCells(cells), combo.Print()),
						(ReadOnlySpan<Cell>)cells);
					return true;
				}
			}
		}
		else
		{
			for (int i = loop == 0 ? 0 : indexes[loop - 1] + 1; i < 9; i++)
			{
				Cell c = region[i];
				if (c.CandI.Count == 0)
				{
					continue;
				}

				cells[loop] = c;
				indexes[loop] = i;

				if (NakedTuple_Recurse(region, amount, loop + 1, cells, indexes))
				{
					return true;
				}
			}
		}
		return false;
	}
}
