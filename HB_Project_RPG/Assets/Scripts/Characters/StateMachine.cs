using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// MonoBehaviour �� StateMachine �� ������ �ֵ��� ���
// StateMachine ���� ����� State �� ���� (�߻�ȭ Ŭ����)
public abstract class State<T>
{
    // State �� �����ϴ� StateMachine �� ������ �� �ֵ���
    // T : context �� ���� Ÿ��
    protected StateMachine<T> stateMachine;
    // ���� (������) �� ���� context
    // MonoBehaviour Ÿ���� ���´�.
    protected T context;
    public State()
    {

    }

    // StateMachine �� context �� �����ϴ� �Լ�
    internal void SetStateMachineAndContext(StateMachine<T> stateMachine, T context)
    {
        this.stateMachine = stateMachine;
        this.context = context;

        // ���¿� ���� �ʱ�ȭ �Լ� ȣ��
        OnInitialized();
    }

    public virtual void OnInitialized()
    {

    }
    public virtual void OnEnter()
    {

    }
    // ���� �����Լ�
    // �ݵ�� �����ؾ� �Ѵ�.
    public abstract void OnUpdate(float deltaTime);
    public virtual void OnExit()
    {

    }
}

// �ٸ� Ŭ�������� ������� ���ϵ��� (������ ������)
public sealed class StateMachine<T>
{
    private T context;

    // ���� ���¿� ���� ����
    private State<T> currentState;
    // ������Ƽ �߰�
    // �ܺο����� �Ҵ� �Ұ�, �б� ���� (readonly)
    public State<T> CurrentState => currentState;

    // ��� ���¿��� ����Ǿ����� Ȯ���ϱ� ���� ����
    private State<T> priviousState;
    public State<T> PriviousState => priviousState;

    // ���� ��ȭ �� �󸶳� �ð��� �귶���� Ȯ���ϱ� ���� ����
    private float elapsedTimeInState = 0.0f;
    public float ElapsedTimeInState => elapsedTimeInState;

    // Key -> System.Type, Value -> State<T>
    // ���¸� ����ϱ� ���� ��ųʸ�
    // State �� Ÿ�԰� State �� �ν��Ͻ��� �����Ǿ�����.
    private Dictionary<System.Type, State<T>> states = new Dictionary<System.Type, State<T>>();

    public StateMachine(T context, State<T> initialState)
    {
        this.context = context;

        // ���¸� �ʱ�ȭ
        // �⺻ ���� �߰�
        AddState(initialState);
        // ��ϵ� ���¸� ���� ���·� ������ �� �ٷ� ����� �� �ֵ��� �Ѵ�.
        currentState = initialState;
        currentState.OnEnter();
    }

    public void AddState(State<T> state)
    {
        // ����Ϸ��� ���¿� ����ӽ��� ������ �ִ� �����ڸ� �Ķ���ͷ�
        state.SetStateMachineAndContext(this, context);
        // ��ųʸ��� ���
        states[state.GetType()] = state;
    }

    public void Update(float deltaTime)
    {
        // �� ���¿��� �ð��� �󸶳� �귶���� �ľ� ����
        elapsedTimeInState += deltaTime;
        currentState.OnUpdate(deltaTime);
    }

    // R �� �ݵ�� State<T> ���� ��ӹ��� Ÿ���̾�� �Ѵ�
    public R ChangeState<R>() where R : State<T>
    {
        // Ÿ���� Ȯ���ϱ� ����
        var newType = typeof(R);

        // ���� ��ȯ�Ǿ���� ������ Ÿ�԰� �����ϴٸ�
        // ���� ���¸� �ι� ȣ���ϴ� ���̱� ������ 
        if(currentState.GetType() == newType)
        {
            // ���� ��ȯ�� ���� �ʴ´�.
            return currentState as R;
        }

        if(currentState != null)
        {
            // ���°� �����Ѵٸ� �� ���¸� �����Ѵ�.
            currentState.OnExit();
        }

        // ���¸� ��ü���ش�.
        priviousState = currentState;
        currentState = states[newType];
        currentState.OnEnter();
        elapsedTimeInState = 0.0f;

        return currentState as R;
    }
}
