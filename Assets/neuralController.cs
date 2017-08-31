using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum FitnessMeasure{
	distance,
	distance2byTime,
	distanceByTime
}
	

public class neuralController : MonoBehaviour {

	public int sensorLenght;
	Rigidbody rigidbody;


	public FitnessMeasure fitnessMeasure;

	Vector3 forward;
	Vector3 right;
	Vector3 left;

    public int timeScale;

	public int population;
	public static int staticPopulation;

    public double driveTime = 0;
	public static double steering;
	public static double braking;
	public static double motor;

	public static int generation = 0;
	public double [] points;
	public double [] results;
	double [] sensors;
	public static int currentNeuralNetwork = 0;

	public static float bestDistance = 0;


	Network [] networks;
	RaycastHit hit;

	Vector3 position;

    // Use this for initialization
    void Start()
    {
		int[] parameters = { 3, 5, 2 };
		staticPopulation = population;

        Time.timeScale = timeScale;

        Debug.Log("Generation " + generation);
        rigidbody = GetComponent<Rigidbody>();

        results = new double[2];
        points = new double[population];
        sensors = new double[3];


		//default vector values
        forward = Vector3.forward * 2;
        right = new Vector3(0.4f, 0, 0.7f);
        left = new Vector3(-0.4f, 0, 0.7f);
       
        position = transform.position;
        networks = new Network[population];


        for (int i = 0; i < population; i++)
        {
			networks[i] = new Network(parameters);
        }

    }

	void FixedUpdate()
	{
		sensors [0] = getSensor (left);
		sensors [1] = getSensor (forward);
		sensors [2] = getSensor (right);


		results = networks[currentNeuralNetwork].process(sensors);
		steering =(double) results [0];
		motor = (double) results [1];

		driveTime += Time.deltaTime;

		points[currentNeuralNetwork] += Vector3.Distance(position, transform.position);
		position = transform.position;

	}
	
	// Update is called once per frame
	void Update () {
        
		Time.timeScale = timeScale;
        
		//check if the network is moving
		if(driveTime > 3 && rigidbody.velocity.magnitude<0.005)
        {
			//Debug.Log ("This one stands still!");
            OnCollisionEnter(null);
        }

	}


	//game over, friend :/
	void OnCollisionEnter (Collision col)
	{
		//Debug.Log ("end!");
        resetCarPosition();

		switch(fitnessMeasure)
		{
		case FitnessMeasure.distance2byTime:
			points [currentNeuralNetwork] *= points [currentNeuralNetwork];
			points [currentNeuralNetwork] /= driveTime;
			break;
		case FitnessMeasure.distanceByTime:
			points [currentNeuralNetwork] /= driveTime;
			break;
		default:
			break;
		}

		driveTime = 0;

        //Debug.Log("network " + currentNeuralNetwork + " scored " + points[currentNeuralNetwork]);


		//now we reproduce
        if(currentNeuralNetwork == population-1)
        {
        double maxValue = points[0];
        int maxIndex = 0;

		//looking for the two best networks in the generation

        for(int i = 1; i < population; i++)
        {
            if (points[i] > maxValue)
            {
                maxIndex = i;
                maxValue = points[i];
            }
        }

        Debug.Log("first parent is " + maxIndex);

			if (points [maxIndex] > bestDistance) {
			
				bestDistance = (float)points [maxIndex];
			
			}

            points[maxIndex] = -10;

            Network mother = networks[maxIndex];


            maxValue = points[0];
            maxIndex = 0;

            for (int i = 1; i < population; i++)
            {
                if (points[i] > maxValue)
                {
                    maxIndex = i;
                    maxValue = points[i];
                }
            }

            Debug.Log("second parent is " + maxIndex);

            points[maxIndex] = -10;

            Network father = networks[maxIndex];


            for(int i = 0; i < population; i++)
            {
                points[i] = 0;
				//creating new generation of networks with random combinations of genes from two best parents
                networks[i] = new Network(father, mother);
            }

            generation++;
            Debug.Log("generation " + generation +" is born");

			//because we increment it at the beginning, that's why -1
            currentNeuralNetwork = -1;
        }

        currentNeuralNetwork++;

		//position reset is pretty important, don't forget it :*
        position = transform.position;
	}

	//TODO: sometimes the velocity is not reseted.. for some reason
    void resetCarPosition()
    {
        rigidbody.velocity = Vector3.zero;
        transform.position = new Vector3(0, 1, 0);
        transform.rotation = new Quaternion(0, 0, 0, 0);

    }


		

	double getSensor(Vector3 direction)
	{
		Vector3 fwd = transform.TransformDirection(direction);

		if (Physics.Raycast (transform.position, fwd, out hit)) {
			if (hit.distance < sensorLenght) {
				Debug.DrawRay (transform.position, fwd * sensorLenght, Color.red, 0, true);
				return 1f - hit.distance / 15f;
			} else {
				Debug.DrawRay (transform.position, fwd * sensorLenght, Color.green, 0, true);
			}
		}
		else
			Debug.DrawRay(transform.position, fwd * sensorLenght, Color.green, 0, true);

		return 0;
	}


}

