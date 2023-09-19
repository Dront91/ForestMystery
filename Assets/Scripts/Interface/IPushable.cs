using System.Collections;
using UnityEngine;

namespace MysteryForest
{
    public interface IPushable
    {
        // obj - ссылка на объект который толкается(ссылка на свой компонент)
        // direction - Направление толчка
        // pushForce - От этого параметра зависит насколько далеко будет подвинут объект, берется от того объекта который вызвал толчок
        // pushingTime - рекомендуется значение 1f, чем больше значение, тем медленнее будет отлетать объект
        bool IsPushing { get; set; }
        void StartPushing(MonoBehaviour obj, Vector2 direction, float pushForce, float pushingTime)
        {
            if (IsPushing == false && obj != null)
            {
                obj.StartCoroutine(PushBack(obj, direction, pushForce, pushingTime));
            }
        }
        IEnumerator PushBack(MonoBehaviour obj, Vector2 direction, float pushForce, float pushingTime)
        {
            IsPushing = true;
            float countTime = 0;
            var startPosition = obj.transform.position;
            var endPosition = (Vector2)obj.transform.position + direction.normalized * pushForce;
            while (countTime <= pushingTime)
            {
                if (obj == null)
                {
                    IsPushing = false;
                    yield break;
                }
                float percentTime = countTime / pushingTime;
                obj.transform.position = Vector2.Lerp(startPosition, endPosition, percentTime);
                yield return null;
                countTime += Time.deltaTime;
            }
            if (obj == null)
            {
                IsPushing = false;
                yield break;
            }
            obj.transform.position = endPosition;
            IsPushing = false;
        }
    }
}
