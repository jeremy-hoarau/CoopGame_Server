using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ObjectType
{
    door,
    button,
    switch_,
    elevator,
    platform,
    box
}

public class ServerSend
    {
        private static void SendTCPData(int _toClient, Packet _packet)
        {
            _packet.WriteLength();
            Server.clients[_toClient].tcp.SendData(_packet);
        }

        private static void SendUDPData(int _toClient, Packet _packet)
        {
            _packet.WriteLength();
            Server.clients[_toClient].udp.SendData(_packet);
        }

        private static void SendTCPDataToAll(Packet _packet)
        {
            _packet.WriteLength();
            for (int i = 1; i <= Server.MaxPlayers; i++)
            {
                Server.clients[i].tcp.SendData(_packet);
            }
        }
        
        private static void SendTCPDataToAll(int _exceptClient, Packet _packet)
        {
            _packet.WriteLength();
            for (int i = 1; i <= Server.MaxPlayers; i++)
            {
                if (i != _exceptClient)
                {
                    Server.clients[i].tcp.SendData(_packet);
                }
            }
        }
        
        private static void SendUDPDataToAll(Packet _packet)
        {
            _packet.WriteLength();
            for (int i = 1; i <= Server.MaxPlayers; i++)
            {
                Server.clients[i].udp.SendData(_packet);
            }
        }
        
        private static void SendUDPDataToAll(int _exceptClient, Packet _packet)
        {
            _packet.WriteLength();
            for (int i = 1; i <= Server.MaxPlayers; i++)
            {
                if (i != _exceptClient)
                {
                    Server.clients[i].udp.SendData(_packet);
                }
            }
        }
        
        #region Packets
        public static void Welcome(int _toClient, string _msg)
        {
            using (Packet _packet = new Packet((int) ServerPackets.welcome))
            {
                _packet.Write(_msg);
                _packet.Write(_toClient);

                SendTCPData(_toClient, _packet);
            }
        }

        public static void SpawnPlayer(int _toClient, Player _player)
        {
            using (Packet _packet = new Packet((int) ServerPackets.spawnPlayer))
            {
                _packet.Write(_player.id);
                _packet.Write(_player.username);
                _packet.Write(_player.transform.position);
                _packet.Write(_player.transform.rotation);
                
                SendTCPData(_toClient, _packet);
            }
        }

        public static void PlayerPositionCameraPosition(Player _player)
        {
            using (Packet _packet = new Packet((int)ServerPackets.playerPositionCameraPosition))
            {
                _packet.Write(_player.id);
                _packet.Write(_player.transform.position);
                _packet.Write(_player.cameraPosition);
                
                SendUDPDataToAll(_packet);
            }
        }
        
        public static void PlayerRotation(Player _player)
        {
            using (Packet _packet = new Packet((int)ServerPackets.playerRotation))
            {
                _packet.Write(_player.id);
                _packet.Write(_player.transform.rotation);
                
                SendUDPDataToAll(_player.id, _packet);
                //SendUDPDataToAll(_packet);
            }
        }

        public static void PlayerStartGrappling(int _playerId, Vector3 _grapplingPoint)
        {
            using (Packet _packet = new Packet((int) ServerPackets.playerStartGrappling))
            {
                _packet.Write(_playerId);
                _packet.Write(_grapplingPoint);
                
                SendUDPDataToAll(_packet);
            }
        }
        
        public static void PlayerStopGrappling(int _playerId)
        {
            using (Packet _packet = new Packet((int) ServerPackets.playerStopGrappling))
            {
                _packet.Write(_playerId);
                
                SendUDPDataToAll(_packet);
            }
        }

        public static void PlayerGrabbingState(int _playerId, bool _isGrabbing)
        {
            using (Packet _packet = new Packet((int) ServerPackets.playerGrabbingState))
            {
                _packet.Write(_playerId);
                _packet.Write(_isGrabbing);

                SendUDPData(_playerId, _packet);
            }
        }

        public static void OtherPlayerInputs(int _playerInputsId, bool[] _inputs)
        {
            using (Packet _packet = new Packet((int) ServerPackets.otherPlayerInputs))
            {
                _packet.Write(_playerInputsId);
                _packet.Write(_inputs.Length);
                foreach (var _input in _inputs)
                {
                    _packet.Write(_input);
                }
                SendUDPDataToAll(_playerInputsId, _packet);
            }
        }

        public static void RotatePlayer(int _playerId, Quaternion _rotation)
        {
            using (Packet _packet = new Packet((int) ServerPackets.rotatePlayer))
            {
                _packet.Write(_rotation);
                
                SendUDPData(_playerId, _packet);
            }
        }

        public static void PlayerDisconnected(int _playerId)
        {
            using (Packet _packet = new Packet((int) ServerPackets.playerDisconnected))
            {
                _packet.Write(_playerId);
                
                SendTCPDataToAll(_packet);
            }
        }

        public static void LoadScene(int _sceneId)
        {
            using (Packet _packet = new Packet((int) ServerPackets.loadScene))
            {
                _packet.Write(_sceneId);
                
                SendTCPDataToAll(_packet);
            }
        }

        public static void WaitingPlayers()
        {
            using (Packet _packet = new Packet((int) ServerPackets.waitingPlayers))
            {
                SendTCPDataToAll(_packet);
            }
        }
        
        public static void StopWaitingPlayers()
        {
            using (Packet _packet = new Packet((int) ServerPackets.stopWaitingPlayers))
            {
                SendTCPDataToAll(_packet);
            }
        }

        public static void ObjectPosition(ObjectType _objectType, int _objectId, Vector3 _objectPosition)
        {
            using (Packet _packet = new Packet((int) ServerPackets.objectPosition))
            {
                _packet.Write(objects[_objectType]);
                _packet.Write(_objectId);
                _packet.Write(_objectPosition);
                
                SendUDPDataToAll(_packet);
            }
        }
        
        public static void ObjectRotation(ObjectType _objectType, int _objectId, Quaternion _objectRotation)
        {
            using (Packet _packet = new Packet((int) ServerPackets.objectRotation))
            {
                _packet.Write(objects[_objectType]);
                _packet.Write(_objectId);
                _packet.Write(_objectRotation);
                
                SendUDPDataToAll(_packet);
            }
        }

        public static void DestroyObject(ObjectType _objectType, int _objectId)
        {
            using (Packet _packet = new Packet((int) ServerPackets.destroyObject))
            {
                _packet.Write(objects[_objectType]);
                _packet.Write(_objectId);
                
                SendUDPDataToAll(_packet);
            }
        }

        public static void SwapPlatformState(int _id, bool _activated)
        {
            using (Packet _packet = new Packet((int) ServerPackets.swapPlatformState))
            {
                _packet.Write(_id);
                _packet.Write(_activated);
                
                SendTCPDataToAll(_packet);
            }
        }

        
        #endregion
        
        private static readonly Dictionary<ObjectType, int> objects = new Dictionary<ObjectType, int>()
        {
            {ObjectType.button, 1},
            {ObjectType.door, 2},
            {ObjectType.switch_, 3},
            {ObjectType.elevator, 4},
            {ObjectType.platform, 5},
            {ObjectType.box, 6},
        };
    }
