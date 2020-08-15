using UnityEngine;

namespace Assets.Scripts.UnityCode
{
    [RequireComponent(typeof(BoxCollider2D))]
    public class Roadsection : MonoBehaviour
    {
        public float SpawnHeight => (collider.size.y * transform.localScale.y) + transform.position.y;

        private new BoxCollider2D collider;

        private void Awake()
        {
            collider = GetComponent<BoxCollider2D>();
            collider.isTrigger = true;
        }
    }
}
