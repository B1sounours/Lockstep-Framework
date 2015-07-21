﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
namespace Lockstep {
	public static class Partition {
		#region Settings
		public const int Count = 18;
		public const int ShiftSize = FixedMath.SHIFT_AMOUNT + 2;
		public const long OffsetX = FixedMath.One * -36 ;
		public const long OffsetY = FixedMath.One * -36 ;
		#endregion

		public static PartitionNode[] Nodes = new PartitionNode[Count * Count];
		public static LSBody Body;


		static long i,j,k,o,p;
		public static void Initialize ()
		{
			for (i = 0; i < Count * Count; i++)
			{
				Nodes[i] = new PartitionNode ();
			}
		}

		static long GridXMin, GridXMax, GridYMin, GridYMax;
		public static void PartitionObject ()
		{


			GridXMin = Body.XMin <= Body.FutureXMin ? ((Body.XMin - OffsetX) >> ShiftSize) : ((Body.FutureXMin - OffsetX) >> ShiftSize);
			 GridXMax = Body.XMax >= Body.FutureXMax ? ((Body.XMax - OffsetX) >> ShiftSize) : ((Body.FutureXMax - OffsetX) >> ShiftSize);
			 GridYMin = Body.YMin <= Body.FutureXMin ? ((Body.YMin - OffsetY) >> ShiftSize) : ((Body.FutureYMin - OffsetY) >> ShiftSize);
			 GridYMax = Body.YMax >= Body.FutureYMax ? ((Body.YMax - OffsetY) >> ShiftSize) : ((Body.FutureYMax - OffsetY) >> ShiftSize);
			#if UNITY_EDITOR
			if (GridXMin < 0 || GridXMax >= Count || GridYMin < 0 || GridYMax >= Count)
			{
				Debug.LogError ("Object with ID " + Body.ID.ToString () + " is out of partition bounds.");
				return;
			}
			#endif
			if (Body.PastGridXMin != GridXMin ||
				Body.PastGridXMax != GridXMax ||
			    Body.PastGridYMin != GridYMin ||
			    Body.PastGridYMax != GridYMax)
			{
				for (o = Body.PastGridXMin; o <= Body.PastGridXMax; o++)
				{
					for (p = Body.PastGridYMin; p <= Body.PastGridYMax; p++)
					{
						Nodes[o * Count + p].Remove (Body.ID);
					}
				}

				for (i = GridXMin; i <= GridXMax; i++)
				{
					for (j = GridYMin; j <= GridYMax; j++)
					{
						Nodes[i * Count + j].Add (Body.ID);
					}
				}


				Body.PastGridXMin = GridXMin;
				Body.PastGridXMax = GridXMax;
				Body.PastGridYMin = GridYMin;
				Body.PastGridYMax = GridYMax;
			}



		}
		static PartitionNode node;
	
		static int ListLength, id1, id2;
		static CollisionPair pair;
		public static void EstablishPartitions ()
		{
			for (i = 0 ; i < Count * Count; i++)
			{
				 node = Nodes[i];
				ListLength = node.Count;
				if (ListLength == 0) continue;
				for (j = 0; j < ListLength; j++)
				{
					id1 = node.innerArray[j];
					for (k = j + 1; k < ListLength; k++)
					{
						id2 = node.innerArray[k];
						if (id1 < id2)
						{
							pair = PhysicsManager.CollisionPairs[id1 * PhysicsManager.MaxSimObjects + id2];
						}
						else {
							pair = PhysicsManager.CollisionPairs[id2 * PhysicsManager.MaxSimObjects + id1];
						}
						if (pair == null) continue;
						pair.SamePartition = true;
					}
				}
			}
		}

	}
	public enum Quadrant : int {
		RightTop,
		RightBot,
		LeftBot,
		LeftTop
	}

}