using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Manager : MonoBehaviour
{
    [SerializeField] private GameObject birdPref;
    [SerializeField] private GameObject[] pipes;
    [SerializeField] private int populationSize;
    [SerializeField] private float mutationChance;
    [SerializeField] private Text infoText;
    [SerializeField] [Range(1f, 10f)] float timeScale = 1f;

    private float maxFitness;
    private int maxScore;
    private int generation;
    private int deadCount = 0;
    private List<Player> lastPopulation = new List<Player>();
    private List<Player> birdList = new List<Player>();
    private int[] layers;

    public GameObject[] Pipes { get => pipes; }

    public void BirdCrashed()
    {
        deadCount++;
        if (deadCount == populationSize)
            EndGeneration();
    }

    private void Start()
    {
        layers = new int[] { 4, 8, 8, 1 };
        InstantiatePopulation();
    }

    private void Update()
    {
        Time.timeScale = timeScale;
        UpdateScore();
    }

    private void FixedUpdate()
    {
        /*for (int i = 0; i < populationSize - 1; i++)
            birdList[i].Brain.CompareTo(birdList[i + 1].Brain);*/
        birdList.Sort(SortByFitness);

        if (birdList.Count > 0)
        {
            maxFitness = Math.Max(birdList[0].Brain.fitness, maxFitness);
            maxScore = Math.Max(birdList[0].Score, maxScore);
        }
    }

    private void EndGeneration()
    {
        pipes[0].GetComponentInChildren<Pipe1>().IsUpdate = true;
        pipes[1].GetComponentInChildren<Pipe1>().IsUpdate = true;
        pipes[2].GetComponentInChildren<Pipe1>().IsUpdate = true;
        pipes[3].GetComponentInChildren<Pipe1>().IsUpdate = true;

        DestroyBirdBodies();
        StartGeneration();
    }

    private void StartGeneration()
    {
        generation++;
        deadCount = 0;
        lastPopulation = birdList;
        birdList = new List<Player>();
        InstantiatePopulation();
    }

    private void DestroyBirdBodies()
    {
        for (int i = 0; i < birdList.Count; i++)
            Destroy(birdList[i].gameObject);
    }

    private void InstantiateBird()
    {
        Player bird = Instantiate(birdPref, new Vector3(-1.4f, 0f, 0f), Quaternion.identity).GetComponent<Player>();
        birdList.Add(bird);
    }

    private int SortByFitness(Player a, Player b)
    {
        return -(a.Brain.fitness.CompareTo(b.Brain.fitness));
    }

    private void InstantiatePopulation()
    {
        birdList = new List<Player>();

        for (int i = 0; i < populationSize; i++)
        {
            InstantiateBird();

            if (generation == 0)
                birdList[i].SetBrain(new NeuralNetwork(layers));
            else
                MutateLastPopulation(i);
        }
    }

    private void MutateLastPopulation(int i)
    {
        int top = 3;
        if (i < top)
        {
            //NeuralNetwork copy = new NeuralNetwork(lastPopulation[i].Brain.Layers);
            NeuralNetwork copy = lastPopulation[i].Brain;
            birdList[i].SetBrain(copy);
        }
        else if (i < populationSize * 0.25f) 
        {
            //NeuralNetwork copy = new NeuralNetwork(lastPopulation[i].Brain.Layers);
            NeuralNetwork copy = lastPopulation[i].Brain;
            copy.Mutate(mutationChance, 0.2f);
            birdList[i].SetBrain(copy);
        }
        else if (i < populationSize * 0.50f)
        {
            //NeuralNetwork copy = new NeuralNetwork(lastPopulation[i % top].Brain.Layers);
            NeuralNetwork copy = lastPopulation[i % top].Brain;
            copy.Mutate(mutationChance, 0.2f);
            birdList[i].SetBrain(copy);
        }
        else if (i < populationSize * 0.75f)
        {
            //NeuralNetwork copy = new NeuralNetwork(lastPopulation[i % top].Brain.Layers);
            NeuralNetwork copy = lastPopulation[i % top].Brain;
            copy.Mutate(mutationChance * 0.5f, 0.2f);
            birdList[i].SetBrain(copy);
        }
        else if (i < populationSize * 1.00f)
        {
            //NeuralNetwork copy = new NeuralNetwork(lastPopulation[i % top].Brain.Layers);
            NeuralNetwork copy = lastPopulation[i % top].Brain;
            copy.Mutate(mutationChance * 2f, 0.2f);
            birdList[i].SetBrain(copy);
        }
    }

    private void UpdateScore()
    {
        infoText.text = "Gen: " + generation + "\n" + "Best: " + maxScore + "\n" + "Fit: " + maxFitness;
    }
}
