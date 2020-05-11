cd Assets/Resources/Brains
mlagents-learn trainer_config.yaml --curriculum curricula/curricula.yaml --env ../../../Builds/Training/PacBot --run-id=PacBot --resume
pause