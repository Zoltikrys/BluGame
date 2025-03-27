using UnityEngine;

public class Crane : MonoBehaviour
{
    public Vector2 xBounds {get; set;}
    public Vector2 yBounds {get; set;}
    public LineRenderer lineRenderer;
    private CraneDirection Direction {get; set;}
    private Vector3 MovementDirection {get; set;} = new Vector3();
    private float DescendSpeed {get; set;}
    private float MovementSpeed {get; set;}
    private float MaxDepth {get; set;}
    private float MaxHeight{get; set;}

    void Start()
    {
        lineRenderer = this.GetComponent<LineRenderer>();
    }

    void Update()
    {
        if(!lineRenderer) return;
        lineRenderer.SetPosition(0, new Vector3(transform.position.x, 30, transform.position.z));
        lineRenderer.SetPosition(1, transform.position);

        if(Direction == CraneDirection.DOWN && transform.position.y > MaxDepth){
            Debug.Log($"Set movement direction to: {MovementDirection}");
            MovementDirection = new Vector3(MovementDirection.x, -DescendSpeed, MovementDirection.z);
        }
        else if(Direction == CraneDirection.UP && transform.position.y < MaxHeight){
            MovementDirection = new Vector3(MovementDirection.x, DescendSpeed, MovementDirection.z);
        }
        else MovementDirection = new Vector3(MovementDirection.x, 0, MovementDirection.z);

        //if(MovementDirection.x < xBounds.x &&  MovementDirection.x > xBounds.y) MovementDirection = new Vector3()

        Debug.Log($"Set movement direction to: {MovementDirection}");
        transform.position = Vector3.Lerp(transform.position, transform.position + MovementDirection, Time.deltaTime);

    }

    public void SetCrane(CraneDirection direction, float descendSpeed, float movementSpeed, int maxDepth, int maxHeight){
        Direction = direction;
        DescendSpeed = descendSpeed;
        MovementSpeed = movementSpeed;
        MaxDepth = maxDepth;
        MaxHeight = maxHeight;

        Debug.Log($"{Direction} {DescendSpeed} {MovementSpeed} {MaxDepth} {MaxHeight}");
    }
}
