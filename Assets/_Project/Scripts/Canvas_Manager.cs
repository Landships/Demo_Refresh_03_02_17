using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Canvas_Manager : MonoBehaviour {

    public GameObject server_client_menu;
    public GameObject server_lobby;
    public GameObject join_lobby;
    public GameObject custom_network_manager;
    public GameObject waiting;
    public spawner_manager spawner;
    static bool is_a_host = false;
    static string ip_address;
    static string inserted_ip = "";

    void Start()
    {
       Instantiate(server_client_menu, transform.position, Quaternion.identity);
    }

	public void change_to_host(GameObject server_client)
    {
        is_a_host = true;
        ip_address = Network.player.ipAddress;
        Debug.Log(ip_address);

        Debug.Log("Creating a Game");
        Destroy(server_client);
        Debug.Log("Destroyed Canvas");
        Instantiate(server_lobby, transform.position, Quaternion.identity);
        GameObject s_lobby = GameObject.Find("Server Lobby(Clone)");
        GameObject s_lobby_button = s_lobby.transform.Find("Button").gameObject;
        GameObject s_lobby_button_text = s_lobby_button.transform.Find("Text").gameObject;
        Text ip_to_display = s_lobby_button_text.GetComponent<Text>();
        ip_to_display.text = "IP Address: " + ip_address;

        Instantiate(custom_network_manager, transform.position, Quaternion.identity);
    }

    public void change_to_join(GameObject server_client)
    {
        is_a_host = false;
        Debug.Log("Joining a Game");
        Destroy(server_client);
        Debug.Log("Destroyed Canvas");
        Instantiate(join_lobby, transform.position, Quaternion.identity);
        //Instantiate(custom_network_manager, transform.position, Quaternion.identity);
    }

    public void insert_ip()
    {
        GameObject lobby = GameObject.Find("Join Lobby(Clone)");
        GameObject panel = lobby.transform.Find("Panel").gameObject;
        GameObject input_field = panel.transform.Find("InputField").gameObject;
        GameObject text = input_field.transform.Find("Text").gameObject;
        Text give_ip = text.GetComponent<Text>();
        inserted_ip = give_ip.text;
        //Debug.Log(inserted_ip);
        Instantiate(custom_network_manager, transform.position, Quaternion.identity);
    }

    public void waiting_in_lobby(byte players)
    {
        GameObject lobby = GameObject.Find("Join Lobby(Clone)");
        Destroy(lobby);
        Instantiate(waiting, transform.position, Quaternion.identity);
        GameObject wait = GameObject.Find("Client Waiting(Clone)");
        GameObject panel = wait.transform.Find("Panel").gameObject;
        GameObject wait_panal = panel.transform.Find("Wait").gameObject;
        GameObject wait_text = wait_panal.transform.Find("Text").gameObject;
        Text give_ip = wait_text.GetComponent<Text>();
        give_ip.text = "Player " + players.ToString() + " in Lobby...";


        GameObject n_manager = GameObject.Find("Custom Network Manager(Clone)");
        network_manager n_manager_script = n_manager.GetComponent<network_manager>();
        n_manager_script.started = true;

    }

    public void start_the_game()
    {
        // The game has started!!!
        Debug.Log("THE GAME HAS STARTED!");
        GameObject s_lobby = GameObject.Find("Server Lobby(Clone)");
        Destroy(s_lobby);

        GameObject n_manager = GameObject.Find("Custom Network Manager(Clone)");
        network_manager n_manager_script = n_manager.GetComponent<network_manager>();
        //n_manager_script.game_ready = true;
        
        if (!n_manager_script.is_the_host())
        {
            GameObject wait = GameObject.Find("Client Waiting(Clone)");
            Destroy(wait);
        }
        byte num_players = (is_a_host ? n_manager_script.getServerPlayersAmt() : n_manager_script.client_players_amount);
        spawner.spawn_four_players(num_players);

        //n_manager_script.game_ready = true;
    }


    public string get_address()
    {
        return ip_address;
    }


    public bool get_host_status()
    {
        return is_a_host;
    }

    public string get_inserted_ip()
    {
        return inserted_ip;
    }


}
