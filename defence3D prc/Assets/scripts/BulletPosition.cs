using UnityEngine;

public class BulletPosition : MonoBehaviour {
    public static Transform[] firepoints;

    void Awake () {

		firepoints = new Transform[transform.childCount];
		for (int i = 0; i < firepoints.Length; i++) {
			firepoints[i] = transform.GetChild(i);
		}
    }
}
