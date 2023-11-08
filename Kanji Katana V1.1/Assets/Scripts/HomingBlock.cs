using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using System;
using UnityEditor;

public class HomingBlock : MonoBehaviour
{
    [Header("REFERENCES")]
    [SerializeField] private Rigidbody rb;
    private Rigidbody target;
    [SerializeField] private GameObject explosionPrefab;
    [SerializeField] private GameObject yellowBlock;

    [Header("START")]
    [SerializeField] private float upwardsHeight = 10;
    [SerializeField] private float randomSpawnRange=10;
    [SerializeField] private float randomHeightMultiplierMin = .5f;
    [SerializeField] private float randomHeightMultiplierMax = 2f;
    private Vector3 randDirectionVector;

    [Header("MOVEMENT")]
    [SerializeField] private float speed = 15;
    [SerializeField] private float rotateSpeed = 95;

    [Header("PREDICTION")]
    [SerializeField] private float maxDistancePredict = 100;
    [SerializeField] private float minDistancePredict = 5;
    [SerializeField] private float maxTimePrediction = 5;
    private Vector3 standardPrediction, deviatedPrediction;

    [Header("DEVIATION")]
    [SerializeField] private float deviationAmount = 50;
    [SerializeField] private float deviationSpeed = 2;



    private float startTime;

    private void Awake()
    {
        target = GameObject.Find("Player").GetComponent<Rigidbody>();
        startTime = Time.time;
        randDirectionVector = transform.up * upwardsHeight *Random.Range(randomHeightMultiplierMin,randomHeightMultiplierMax)+ new Vector3(Random.Range(-randomSpawnRange, randomSpawnRange), 0f, Random.Range(-randomSpawnRange, randomSpawnRange));
        rb.AddForce(randDirectionVector, ForceMode.Impulse);
        
    }
    private void FixedUpdate()
    {

        if (!(Time.time - startTime < .5))
        {
            rb.velocity = transform.forward * speed;

            var leadTimePercentage = Mathf.InverseLerp(minDistancePredict, maxDistancePredict, Vector3.Distance(transform.position, target.transform.position));
            

            PredictMovement(leadTimePercentage);

            AddDeviation(leadTimePercentage);

            RotateRocket();
        }
        else
        {
            transform.LookAt(transform.position + randDirectionVector);
        }
    }

    private void PredictMovement(float leadTimePercentage)
    {
        var predictionTime = Mathf.Lerp(0, maxTimePrediction, leadTimePercentage);

        standardPrediction = target.position + target.velocity * predictionTime;
    }

    private void AddDeviation(float leadTimePercentage)
    {
        var deviation = new Vector3(Mathf.Cos(Time.time * deviationSpeed), 0, 0);

        var predictionOffset = transform.TransformDirection(deviation) * deviationAmount * leadTimePercentage;

        deviatedPrediction = standardPrediction + predictionOffset;
    }

    private void RotateRocket()
    {
        var heading = deviatedPrediction - transform.position;

        var rotation = Quaternion.LookRotation(heading);
        rb.MoveRotation(Quaternion.RotateTowards(transform.rotation, rotation, rotateSpeed * Time.deltaTime));
    }
    private void OnCollisionEnter(Collision collision)
    {

        if (explosionPrefab) Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        //if (collision.transform.TryGetComponent<IExplode>(out var ex)) ex.Explode();

        Destroy(yellowBlock);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, standardPrediction);
        Gizmos.color = Color.green;
        Gizmos.DrawLine(standardPrediction, deviatedPrediction);
    }
}

