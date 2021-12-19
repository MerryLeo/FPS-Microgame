using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FakeProjectile : MonoBehaviour
{
    float _speed = 100f, _count;
    Vector3 from, to;

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.Lerp(from, to, _count);
        _count += _speed * Time.deltaTime;
        if (transform.position == to)
        {
            gameObject.SetActive(false);
        }
    }

    public void SendProjectile(Vector3 from, Vector3 to)
    {
        _count = 0;
        _speed = 85f / Vector3.Distance(from, to);
        transform.position = this.from = from;
        transform.forward = to - from;
        this.to = to;
    }
}
