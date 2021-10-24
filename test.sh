#!/bin/bash
echo "Building and running tests..."

sudo docker build . -f Dockerfile.tests 
