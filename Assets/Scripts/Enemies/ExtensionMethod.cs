using UnityEngine;

namespace MysteryForest
{
    public static class ExtensionMethod
    {
        public static Object Instantiate(
            this Object thisObj, Object original, Vector3 position, Quaternion rotation, Collider2D collider, Vector2 target, int damage, float pushForce, float spriteAngle, SoundPlayer soundPlayer)
        {
            GameObject projectileObject = Object.Instantiate(original, position, rotation) as GameObject;

            if (!projectileObject.TryGetComponent<Projectile>(out var projectile))
            {
                Debug.LogWarning("Projectale component absent.");
                return null;
            }
            projectile.SetPushForce(pushForce);
            projectile.SetDestination(target);
            projectile.SetParentCollider(collider);
            projectile.SetDamage(damage);
            projectile.SetSpriteDirection(spriteAngle);
            projectile.SetSoundPlayer(soundPlayer);

            return projectileObject;
        }
    }
}
