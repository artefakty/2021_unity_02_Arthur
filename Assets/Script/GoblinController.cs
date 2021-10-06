using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

public class GoblinController : MonoBehaviour
{
    private float azimuth;

    public enum State
    {
        Standing = 0,
        Walking = 1
    }

    public State CharacterState;
    public bool IsAlerted;
    public bool IsSelected;

    private Animator animator;
    private Vector3 target;

    private void Start()
    {
        animator = this.GetComponent<Animator>();
        azimuth = transform.rotation.eulerAngles.y;
        target = transform.position;

        CharacterState = State.Standing;

        IsAlive = true;
        IsSelected = false;
    }

    public float RotationSpeed = 360.0f;
    public float WalkSpeed = 10.0f;
    public float RunSpeed = 10.0f;
    public float MinDistance = 0.1f;

    public float MinWanderDistance = 1.5f;
    public float MaxWanderDistance = 5.0f;

    private bool IsAlive;

    private void Update()
    {
        animator.SetBool("IsAlive", IsAlive);
        if (IsAlive)
        {
            var distance = target - this.transform.position;

            foreach (var mesh in GetComponentsInChildren<SkinnedMeshRenderer>())
            {
                var materialPropertyBlock = new MaterialPropertyBlock();

                if (IsSelected)
                    materialPropertyBlock.SetColor("_EmissionColor", new Color(0.2f, 0.2f, 0.2f));
                else
                    materialPropertyBlock.SetColor("_EmissionColor", new Color(0, 0, 0));

                mesh.SetPropertyBlock(materialPropertyBlock);
            }
            foreach (var mesh in GetComponentsInChildren<MeshRenderer>())
            {
                var materialPropertyBlock = new MaterialPropertyBlock();

                if (IsSelected)
                    materialPropertyBlock.SetColor("_EmissionColor", new Color(0.2f, 0.2f, 0.2f));
                else
                    materialPropertyBlock.SetColor("_EmissionColor", new Color(0, 0, 0));

                mesh.SetPropertyBlock(materialPropertyBlock);
            }

            if (distance.sqrMagnitude > MinDistance * MinDistance)
            {
                var desiredAzimuth = Mathf.Atan2(distance.x, distance.z) * Mathf.Rad2Deg;

                azimuth = Mathf.MoveTowardsAngle(azimuth, desiredAzimuth, RotationSpeed * Time.deltaTime);

                CharacterState = State.Walking;

                if(IsAlerted)
                    GetComponent<Rigidbody>().position += transform.forward * RunSpeed * Time.deltaTime;
                else
                    GetComponent<Rigidbody>().position += transform.forward * WalkSpeed * Time.deltaTime;
            }
            else
            {
                if (!IsAlerted)
                {
                    target = transform.position + Quaternion.Euler(0, Random.Range(0.0f, 360.0f), 0) * Vector3.forward * Random.Range(MinWanderDistance, MaxWanderDistance);
                }
                CharacterState = State.Standing;
            }

            if (IsAlerted)
            {
                target = FindObjectOfType<CharacterController>().transform.position;
            }

            animator.SetBool("IsAlerted", IsAlerted);
            animator.SetInteger("State", (int) CharacterState);
        }
        transform.rotation = Quaternion.Euler(0, azimuth, 0);
    }
}
