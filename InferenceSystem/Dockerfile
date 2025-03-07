FROM ubuntu:18.04

# Install sudo apt-get requirements
RUN apt-get update && apt-get install -y \
        libsm6 \
        libxext6 \
        libxrender-dev \
        libglib2.0-0 \
        ffmpeg \
        python3 \
        python3-pip \
        unzip \
        git

# Install pip requirements
WORKDIR /usr/src/install
COPY ./requirements.txt ./requirements.txt
RUN python3 -m pip install --upgrade pip && pip install -r requirements.txt

# From now on, we will be working in the app's running directory
WORKDIR /usr/src/app

# Put model in place at WORKDIR/model
COPY ./podcastmodel.zip ./podcastmodel.zip
RUN unzip ./podcastmodel.zip
RUN rm -rf ./__MACOSX
RUN rm ./podcastmodel.zip

# Move source code to app working directory
COPY ./src/ ./src
COPY ./config/ ./config

CMD ["python3", "-u", "./src/LiveInferenceOrchestrator.py", "--config", "./config/AudioSet_LiveHLS_OrcasoundLab.yml"]
