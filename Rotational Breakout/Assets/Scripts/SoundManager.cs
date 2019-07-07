using System.Collections;
using UnityEngine;

public class SoundManager : MonoBehaviour {

	private AudioSource musicAudioSource;               //The two Audiosources used for the background music.

	private float masterVolume = 1f;
	private float musicVolume = .5f;
	private float effectsVolume = .5f;

	private bool masterSoundEnabled = true;
	private bool musicSoundEnabled = true;
	private bool effectSoundEnabled = true;

	[Tooltip ("soundFadeSpeed is de snelheid waarmee geluid fade (hoger getal betekend kortere fade in/out tijd).")]
	[SerializeField] private float soundFadeTime = 1f;


	private void Awake () {
		musicAudioSource = this.gameObject.GetComponent<AudioSource> ();

		//Spawning of the two music AudioSources
		musicAudioSource.loop = true;
		musicAudioSource.mute = true;
		musicAudioSource.volume = 0;
		musicAudioSource.playOnAwake = false;
	}


	///<summary>
	/// Changes the volume (in percentages) on a specific audio type. 
	/// int Channel is the sound channel, '0' > Master; '1' > Music; '2' > Effects; '3' > Voice; '4' > Ambience.
	///</summary>
	public void ChangeSpecificVolume (float volume, int channel) {
		switch (channel) {
			case 0:
				ChangeVolume ((volume / 100), musicVolume, effectsVolume);
				break;
			case 1:
				ChangeVolume (masterVolume, (volume / 100), effectsVolume);
				break;
			case 2:
				ChangeVolume (masterVolume, musicVolume, (volume / 100));
				break;
		}
	}

	///<summary>
	/// Changes the volume (in percentages) off the audio types.
	///</summary>
	public void ChangeVolume (float master, float music, float effects) {
		masterVolume = (master / 100);
		effectsVolume = (effects / 100);
		musicVolume = (music / 100);

		if ((masterVolume == 0 || musicVolume == 0) && musicAudioSource.volume != 0) {
			MuteAudioSource (1);
		}
		else if ((masterSoundEnabled == true && musicSoundEnabled == true) && musicAudioSource.volume == 0) {
			ResumeAudioSource (1);
		}
		else if (musicAudioSource.volume != masterVolume * musicVolume) {
			musicAudioSource.volume = masterVolume * musicVolume;
		}
	}

	///<summary>
	/// MuteChannels mutes or enables certain types of sound.
	///</summary>
	public void MuteChannels (bool master, bool music, bool effects, bool voice, bool ambience) {
		if (musicAudioSource.isPlaying == false && (music == true && master == true)) {
			ResumeAudioSource ();
		}
		else if (musicAudioSource.isPlaying && (music == false || master == false)) {
			MuteAudioSource ();
		}

		masterSoundEnabled = master;
		musicSoundEnabled = music;
		effectSoundEnabled = effects;
	}

	///<summary>
	/// PlayBackgroundSound takes a music clip, and plays that as the background music.
	///</summary>
	public void PlayBackgroundMusic (AudioClip clip, float fadeDuration = 1) {
		MuteAudioSource (fadeDuration);

		musicAudioSource .clip = clip;
		if (musicSoundEnabled && masterSoundEnabled) {
			ResumeAudioSource (fadeDuration);
		}
	}

	///<summary>
	/// PlaySound plays am Audioclip at a specific location.
	///</summary>
	public static void PlaySoundEffect (AudioClip clip, Vector3 pos) {
		if (clip != null) {
			if (GameManager.instance.soundManager.effectSoundEnabled == true) {
				AudioSource.PlayClipAtPoint (clip, pos, GameManager.instance.soundManager.masterVolume * GameManager.instance.soundManager.effectsVolume);
			}
		}
	}

	///<summary>
	/// MuteAudioSource mutes a specific AudioSource.
	///</summary>
	public void MuteAudioSource (float fadeDuration = 1) {
		StopAllCoroutines ();
		StartCoroutine (FadeSound (musicAudioSource, fadeDuration));
	}

	///<summary>
	/// ResumeMusic causes the currently active audioSource to start playing its sound again.
	///</summary>
	public void ResumeAudioSource (float fadeDuration = 1) {
		StopAllCoroutines ();
		StartCoroutine (StartSound (musicAudioSource, fadeDuration));
	}

	//StartMusic takes an AudioSource and plays it, while slowly increasing the volume to the desired amount.
	IEnumerator StartSound (AudioSource a, float fadeDuration) {
		a.mute = false;
		a.Play ();

		while (a.volume < masterVolume * musicVolume) {
			a.volume += Time.deltaTime / fadeDuration;
			yield return null;
		}
		a.volume = masterVolume * musicVolume;
	}

	//FadeMusic takes an AudioSource and stops it from playing, after slowly decreasing the volume to zero.
	IEnumerator FadeSound (AudioSource a, float fadeDuration) {
		while (a.volume > 0) {
			a.volume -= Time.deltaTime / fadeDuration;
			yield return null;
		}

		a.volume = 0;
		a.Stop ();
	}

}
