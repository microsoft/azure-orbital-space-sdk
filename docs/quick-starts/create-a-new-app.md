# Create a new app

We have provided a helper script to streamline creating a new Microsoft Azure Orbital Space SDK Payload app, using our starter-apps as a source.  Run the below script to create a new payload app:

```bash
bash -c "${PWD}/scripts/create-app.sh --output-dir ${PWD}/tmp --app-name AwesomePayloadApp"
```

Now you can run code on your new app and start developing:
```bash
code ${PWD}/tmp/AwesomePayloadApp
```