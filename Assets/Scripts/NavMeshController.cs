using NavMeshPlus.Components;
using UnityEngine;


namespace MysteryForest
{
    public class NavMeshController : MonoBehaviour
    {
        private NavMeshSurface _navMeshSurface;
        void Start()
        {
            _navMeshSurface = GetComponent<NavMeshSurface>();
            _navMeshSurface.BuildNavMeshAsync();
        }
    }
}
