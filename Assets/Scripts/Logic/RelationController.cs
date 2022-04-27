using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RelationController
{
    public Dictionary<(int, int), Relation> Relations = new Dictionary<(int, int), Relation>();

    public RelationController()
    {
        Relations.Add((1, 2), new Relation((1, 2)));
        Relations.Add((1, 3), new Relation((1, 2)));
        Relations.Add((1, 4), new Relation((1, 2)));
        Relations.Add((2, 3), new Relation((1, 2)));
        Relations.Add((2, 4), new Relation((1, 2)));
        Relations.Add((3, 4), new Relation((1, 2)));
    }

    public void LoadRelations(Save save)
    {
        Relations = save.Relations;
    }

    public Relation GetRelation(int p1, int p2)
    {
        return Relations[(Mathf.Min(p1, p2), Mathf.Max(p1, p2))];
    }

    public List<Relation> GetRelationsWithIsland(int index)
    {
        return (from relation in Relations
                let partners = relation.Key
                where partners.Item1 == index || partners.Item2 == index
                select relation.Value).ToList();
    }

    public bool UpdateRelation(int sender, int receiver, int relationDelta)
    {
        return GetRelation(receiver, sender).UpdateRelation(relationDelta);
    }
}