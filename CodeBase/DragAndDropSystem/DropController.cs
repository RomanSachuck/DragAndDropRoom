using DG.Tweening;
using UnityEngine;

namespace CodeBase.DragAndDropSystem
{
    public class DropController
    {
        private const int RoomCollidersLayer = 6;
        
        private readonly int _layerMask = 1 << RoomCollidersLayer;

        public void Drop(CircleCollider2D selfCollider)
        {
            if (TryGetCollisionCollider(selfCollider, out Collider2D collider))
            {
                FallOnTheCurrentCollider(selfCollider, collider);
            }
            else
            {
                if (TryGetLowerCollider(selfCollider, out Collider2D lowerCollider))
                    FallOnTheLowerCollider(selfCollider, lowerCollider);
                else
                    FallOnTheNearbyCollider(selfCollider);
            }
        }

        private void FallOnTheCurrentCollider(CircleCollider2D selfCollider, Collider2D otherCollider)
        {
            Vector3 dropPosition = DeterminePositionOnTheCollider(selfCollider, otherCollider);
            selfCollider.transform.DOMove(dropPosition, 0.5f).SetEase(Ease.OutBounce);
        }

        private void FallOnTheNearbyCollider(CircleCollider2D selfCollider)
        {
            Collider2D otherCollider = FindNearbyColliderAndThrowPosition(selfCollider, out Vector3 throwPosition);

            DOTween.Sequence()
                .Append(selfCollider.transform.DOMove(throwPosition, 0.5f).SetEase(Ease.OutBack))
                .AppendCallback(() => FallOnTheCurrentCollider(selfCollider, otherCollider));
        }

        private void FallOnTheLowerCollider(CircleCollider2D selfCollider, Collider2D lowerCollider)
        {
            float radius = selfCollider.radius * selfCollider.transform.lossyScale.x;
            Vector3 position = selfCollider.transform.position;
            position.y = lowerCollider.transform.position.y + 1;

            while (!ObjectOnTheCollider(lowerCollider, position, radius)) 
                position.y -= 0.05f;

            selfCollider.transform.DOMove(position, 0.5f).SetEase(Ease.OutBounce);
        }

        private bool TryGetCollisionCollider(CircleCollider2D selfCollider, out Collider2D otherCollider)
        {
            float radius = selfCollider.radius * selfCollider.transform.lossyScale.x;
            Vector2 raycastOrigin = new Vector2(selfCollider.transform.position.x,
                selfCollider.transform.position.y - radius);

            RaycastHit2D hit = Physics2D.CircleCast(raycastOrigin, radius / 4, Vector2.down, radius, _layerMask);

            if (hit.collider != null)
            {
                otherCollider = hit.collider;
                return true;
            }

            otherCollider = null;
            return false;
        }

        private bool TryGetLowerCollider(CircleCollider2D selfCollider, out Collider2D otherCollider)
        {
            Collider2D colliderFromBelow =
                Physics2D.Raycast(selfCollider.transform.position, Vector2.down, 10, _layerMask).collider;

            if (colliderFromBelow != null)
            {
                otherCollider = colliderFromBelow;
                return true;
            }

            otherCollider = null;
            return false;
        }

        private Collider2D FindNearbyColliderAndThrowPosition(CircleCollider2D selfCollider,
            out Vector3 throwPosition)
        {
            RaycastHit2D hit;
            float radius = .5f;

            do
            {
                hit = Physics2D.CircleCast(selfCollider.transform.position, radius, Vector2.zero, 0, _layerMask);
                radius += .1f;
            } while (hit.collider == null);

            throwPosition = hit.point;
            return hit.collider;
        }

        private Vector3 DeterminePositionOnTheCollider(CircleCollider2D selfCollider, Collider2D otherCollider)
        {
            float radius = selfCollider.radius * selfCollider.transform.lossyScale.x;
            Vector3 position = selfCollider.transform.position;

            position.y -= 0.5f;

            while (!ObjectOnTheCollider(otherCollider, position, radius)) 
                position.y += 0.05f;

            return position;
        }

        private bool ObjectOnTheCollider(Collider2D otherCollider, Vector3 position, float radius) =>
            Physics2D.CircleCast(new Vector2(position.x, position.y - radius),
                radius / 4, Vector2.down,
                0, _layerMask).collider == otherCollider;
    }
}