import React, { useContext } from 'react';
import { Form, Button } from 'semantic-ui-react';
import { Form as FinalForm, Field } from "react-final-form";
import TextInput from '../../../app/common/form/TextInput';
import TextAreaIput from '../../../app/common/form/TextAreaIput';
import { RootStoreContext } from '../../../app/stores/rootStore';
import { EditProfileFormValues, IProfileFormValues } from '../../../app/models/profile';
import { observer } from 'mobx-react-lite';
import { combineValidators, isRequired } from 'revalidate';

const validate = combineValidators({
    displayName: isRequired('displayName')
});

const ProfileEditForm = () => {
    const rootStore = useContext(RootStoreContext);
    const { profile, editProfile, submitting } = rootStore.profileStore;

    const formValues = profile ? new EditProfileFormValues(profile) : new EditProfileFormValues();

    return (
        <FinalForm
            onSubmit={(values: IProfileFormValues) => editProfile(values)}
            validate={validate}
            initialValues={formValues}
            render={({ handleSubmit, pristine, invalid }) => (
                <Form onSubmit={handleSubmit} >
                    <Field
                        name='displayName'
                        component={TextInput}
                    />
                    <Field
                        name='bio'
                        component={TextAreaIput}
                    />
                    <Button
                        loading={submitting}
                        disabled={pristine || invalid}
                        type='submit'
                        positive
                        floated='right'
                        content='Update profile' />
                </Form>
            )} />
    )
}

export default observer(ProfileEditForm);
