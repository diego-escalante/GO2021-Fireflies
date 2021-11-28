using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Fader : MonoBehaviour {

    [SerializeField] private float FadeTime = 3f;

    private RawImage image;
    private Coroutine co;
    
    private void Awake() {
        image = GetComponent<RawImage>();
        image.color = Color.black;
    }

    public void FadeIn() {
        if (co != null) {
            StopCoroutine(co);
        }
        co = StartCoroutine(Fade(true, FadeTime));
    }

    public void FadeOut() {
        if (co != null) {
            StopCoroutine(co);
        }
        co = StartCoroutine(Fade(false, FadeTime));
    }

    private IEnumerator Fade(bool fadingIn, float duration) {
        Color startingColor = image.color;
        float timeLeft = duration;
        while (timeLeft >= 0) {
            timeLeft -= Time.deltaTime;
            image.color = Color.Lerp(startingColor, fadingIn ? Color.clear : Color.black,
                (duration - timeLeft) / duration);
            yield return null;
        }
        co = null;
    }
}
