// <copyright file="AudioManager.cs" company="Up Up Down Studios">
// Copyright (c) 2018 All Rights Reserved
// </copyright>
// <summary>Manager for Unity audio events.</summary>

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manager for Unity audio events.
/// </summary>
public class AudioManager : MonoBehaviour
{
    #region Properties

    public List<AudioSource> music = new List<AudioSource>();
    public List<AudioSource> effects = new List<AudioSource>();

    //Singleton
    public static AudioManager Singleton
    {
        get; private set;
    }

    #endregion

    #region Unity functions

    private void Awake()
    {
        if (Singleton != null)
            DestroyImmediate(gameObject);
        else
            Singleton = this;

        GetReferences();
    }

    #endregion

    #region Class functions

    private void GetReferences() //Improve! --- Async
    {
        AudioSource[] audioSourcesInScene = FindObjectsOfType(typeof(AudioSource)) as AudioSource[];

        foreach (AudioSource audioSource in audioSourcesInScene)
        {
            switch (audioSource.tag)
            {
                case "Music":
                    music.Add(audioSource);
                    break;

                case "Effect":
                    effects.Add(audioSource);
                    break;

                default:
                    break;
            }
        }
    }

    public void Music(bool state)
    {
        foreach (AudioSource audioSource in music)
            audioSource.mute = !state;
    }

    public void Effect(bool state)
    {
        foreach (AudioSource audioSource in music)
            audioSource.mute = !state;
    }

    #endregion
}
