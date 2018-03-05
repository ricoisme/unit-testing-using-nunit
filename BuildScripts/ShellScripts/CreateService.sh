#!/bin/sh
service=@{serviceName}@.service
if (( $(ps -ef | grep -v grep | grep $service | wc -l) > 0 ))
then
sudo systemctl stop $service
sudo systemctl disable $service
rm -rf /usr/lib/systemd/system/$service
else
echo "[Unit]
Description=@{Description}@

[Service]
WorkingDirectory=@{WorkingDirectory}@
ExecStart=/usr/bin/dotnet @{WorkingDirectory}@/@{assembly}@
ExecStop=/usr/bin/dotnet @{WorkingDirectory}@/@{assembly}@
Restart=always
RestartSec=10
SyslogIdentifier=dotnetwebapi-demo
Environment=ASPNETCORE_ENVIRONMENT=@{Environment}@
User=jenkins

[Install]
WantedBy=multi-user.target" > /usr/lib/systemd/system/$service

fi

sudo systemctl daemon-reload






