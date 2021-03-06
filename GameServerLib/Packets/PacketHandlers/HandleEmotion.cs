﻿using ENet;
using GameServerCore.Packets.Enums;
using LeagueSandbox.GameServer.API;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using LeagueSandbox.GameServer.Logging;
using LeagueSandbox.GameServer.Players;
using log4net;

namespace LeagueSandbox.GameServer.Packets.PacketHandlers
{
    public class HandleEmotion : PacketHandlerBase
    {
        private readonly Game _game;
        private readonly PlayerManager _playerManager;
        private readonly ILog _logger;

        public override PacketCmd PacketType => PacketCmd.PKT_C2S_EMOTION;
        public override Channel PacketChannel => Channel.CHL_C2S;

        public HandleEmotion(Game game)
        {
            _game = game;
            _playerManager = game.PlayerManager;
            _logger = LoggerProvider.GetLogger();
        }

        public override bool HandlePacket(Peer peer, byte[] data)
        {
            var champion = _playerManager.GetPeerInfo(peer).Champion;
            champion.StopChampionMovement();
            var request = _game.PacketReader.ReadEmotionPacketRequest(data);
            //for later use -> tracking, etc.
            var playerName = _playerManager.GetPeerInfo(peer).Champion.Model;
            switch (request.Id)
            {
                case Emotions.DANCE:
                    _logger.Debug("Player " + playerName + " is dancing.");
                    break;
                case Emotions.TAUNT:
                    _logger.Debug("Player " + playerName + " is taunting.");
                    break;
                case Emotions.LAUGH:
                    _logger.Debug("Player " + playerName + " is laughing.");
                    break;
                case Emotions.JOKE:
                    _logger.Debug("Player " + playerName + " is joking.");
                    break;
            }

            _game.PacketNotifier.NotifyEmotions(request.Id, request.NetId);
            return true;
        }
    }
}
