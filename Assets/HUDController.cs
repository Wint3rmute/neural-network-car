using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDController : MonoBehaviour {

	Text generation;
	Text population;
	Text engine;
	Text top;

	// Use this for initialization
	void Start () {
		generation = GetComponent<Text> ();
		population = GameObject.Find ("Population").GetComponent<Text> ();
		engine = GameObject.Find ("speed").GetComponent<Text> ();
		top = GameObject.Find ("top").GetComponent<Text>();
	}
	
	// Update is called once per frame
	void Update () {
		generation.text = "Generation " + (neuralController.generation+1);
		population.text = "Population " + (neuralController.currentNeuralNetwork+1)  + " / " + neuralController.staticPopulation;;
		engine.text = "Engine " + ((float)neuralController.motor).ToString();
		top.text = "Top " + neuralController.bestDistance;

		
	}
}
