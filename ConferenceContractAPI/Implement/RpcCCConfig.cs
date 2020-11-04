﻿using Grpc.Core;
using GrpcConferenceContractService;
using GrpcConferenceContractServiceNew;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ConferenceContractAPI.Implement
{
    public class RpcCCConfig
    {
        private static Server _server;
        public static void Start()
        {
            _server = new Server
            {
                //Services = { ConferenceContractServiceToGrpc.BindService(new CCServiceImpl()) },
                Ports = { new ServerPort("0.0.0.0", 40001, ServerCredentials.Insecure) }
            };

            _server.Services.Add(ConferenceContractServiceToGrpc.BindService(new CCServiceImpl()));
            _server.Services.Add(NewConferenceContractService.BindService(new ConferenceContractAPI.ConferenceContractService.ConferenceContractService()));
            _server.Start();

        }
    }
}
