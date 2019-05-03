﻿using Graft.Primitives;
using System;
using System.Collections.Generic;

namespace Graft
{
    public interface IWeightedGraph<TV, TW> : IGraph<TV> where TV : IEquatable<TV>
    {
        new IEnumerable<IWeightedEdge<TV, TW>> GetAllEdges();

        new IEnumerable<IWeightedEdge<TV, TW>> GetEdgesOfVertex(IVertex<TV> vertex);

        new IWeightedEdge<TV, TW> GetEdgeBetweenVerteces(IVertex<TV> source, IVertex<TV> target);
    }
}
