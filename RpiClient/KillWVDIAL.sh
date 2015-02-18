#!/bin/bash
ps aux | grep -ie wvdial | awk '{print $2}' | xargs kill -9 