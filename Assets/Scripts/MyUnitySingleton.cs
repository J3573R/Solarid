using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MyUnitySingleton : MonoBehaviour {

	private static MyUnitySingleton instance = null;
	private string _nextLevel;
	private AudioSource _source;

	public AudioClip SouthClip;
	public AudioClip EastWestClip;
	public AudioClip NorthClip;
	public AudioClip HubClip;

	public static MyUnitySingleton Instance {
		get { return instance; }
	}

	void Awake() {
		_source = GetComponent<AudioSource> ();

		if (instance != null && instance != this) {
			Destroy(this.gameObject);
			return;
		} else {
			instance = this;
		}
		DontDestroyOnLoad(this.gameObject);
	}		

	public void PlayMusic() {
		string levelName = SceneManager.GetActiveScene().name;
		AudioClip tmp = null;

		if (levelName.Contains ("South"))
			tmp = SouthClip;
		else if (levelName.Contains ("West") || levelName.Contains ("East"))
			tmp = EastWestClip;
		else if (levelName.Contains ("North"))
			tmp = NorthClip;
		else if (levelName.Contains ("Hub"))
			tmp = HubClip;
		else
			_source.Stop ();

		if (_source.clip != tmp) {
			_source.clip = tmp;
			_source.Play ();
		}
	}
}
