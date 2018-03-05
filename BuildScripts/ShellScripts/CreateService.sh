#!/bin/sh
cat /usr/lib/systemd/system/${serviceName}$.service << EOF

[Unit]
Description=${Description}$

[Service]
WorkingDirectory=${WorkingDirectory}$
ExecStart=/usr/bin/dotnet ${WorkingDirectory}$/${assembly}$
ExecStop=/usr/bin/dotnet ${WorkingDirectory}$/${assembly}$
Restart=always
RestartSec=10
SyslogIdentifier=dotnetwebapi-demo
Environment=ASPNETCORE_ENVIRONMENT=${Environment}$
User=jenkins

[Install]
WantedBy=multi-user.target

EOF