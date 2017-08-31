		using System.Collections;
		using System.Collections.Generic;
		using UnityEngine;

		public class Network2 {

			float[,,] Synapse;
			float[] NeuronsMid;

			float sigmoid(float x)
			{
				return 1 / (1 + Mathf.Exp(-x));
			}




			public Network2()
			{
				Synapse = new float[2 , 5 , 5];

		int a = 0;

				//1 warstwa
				for (int i = 0; i < 3; i++)
					for (int j = 0; j < 5; j++)
			{ Synapse [0, i, j] =  RandomWeight();
						//Debug.Log(Synapse[0, i, j]);
					}
				//2 warstwa
				for (int i = 0; i < 5; i++)
					for (int j = 0; j < 2; j++)
			{ Synapse [1, i, j] =  RandomWeight();
						//Debug.Log(Synapse[1, i, j]);
					}



			}

			public float [] process(float[] sensors)
			{
				float Output0;
				float Output1;
				float [] Output = new float[2];


				float[] NeuronsMid = new float[5];
				NeuronsMid[0] = sensors[0] * Synapse[0, 0, 0] + sensors[1] * Synapse[0, 1, 0] + sensors[2] * Synapse[0, 2, 0];
				NeuronsMid[1] = sensors[0] * Synapse[0, 0, 1] + sensors[1] * Synapse[0, 1, 1] + sensors[2] * Synapse[0, 2, 1];
				NeuronsMid[2] = sensors[0] * Synapse[0, 0, 2] + sensors[1] * Synapse[0, 1, 2] + sensors[2] * Synapse[0, 2, 2];
				NeuronsMid[3] = sensors[0] * Synapse[0, 0, 3] + sensors[1] * Synapse[0, 1, 3] + sensors[2] * Synapse[0, 2, 3];
				NeuronsMid[4] = sensors[0] * Synapse[0, 0, 4] + sensors[1] * Synapse[0, 1, 4] + sensors[2] * Synapse[0, 2, 4];

				NeuronsMid[0] = sigmoid(NeuronsMid[0]);
				NeuronsMid[1] = sigmoid(NeuronsMid[1]);
				NeuronsMid[2] = sigmoid(NeuronsMid[2]);
				NeuronsMid[3] = sigmoid(NeuronsMid[3]);
				NeuronsMid[4] = sigmoid(NeuronsMid[4]);


				Output0 = NeuronsMid[0] * Synapse[1, 0, 0] + NeuronsMid[1] * Synapse[1, 1, 0] + NeuronsMid[2] * Synapse[1, 2, 0] + NeuronsMid[3] * Synapse[1, 3, 0] + NeuronsMid[4] * Synapse[1, 4, 0];
				Output1 = NeuronsMid[0] * Synapse[1, 0, 1] + NeuronsMid[1] * Synapse[1, 1, 1] + NeuronsMid[2] * Synapse[1, 2, 1] + NeuronsMid[3] * Synapse[1, 3, 1] + NeuronsMid[4] * Synapse[1, 4, 1];

				Output0 = sigmoid(Output0);
				Output1 = sigmoid(Output1);

				Output[0] = Output0;
				Output[1] = Output1;

				return Output;
			}


		
			float RandomWeight()
			{
				return 1; //Random.Range(-1.0f, 1.0f);              
			}

		}
	