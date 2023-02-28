using UnityEngine;

namespace Pixygon.Micro {
    public class GroundChecker : MonoBehaviour {
        [SerializeField] private LayerMask _groundLayer;
        
        public bool Grounded (float size)
        {
            return Physics2D.OverlapCircle(transform.position, size, _groundLayer);
        }
    }
}