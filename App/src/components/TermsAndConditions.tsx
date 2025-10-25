import React, { useState } from 'react';
import { View, Text, TouchableOpacity, StyleSheet, Linking, ScrollView } from 'react-native';
import { useApi } from '@/hooks/useApi';
import { useAccount } from '@/hooks/useAccount';

const TermsAndConditionsComponent: React.FC = () => {
  const { updateAccount } = useApi();
  const { termsAccepted } = useAccount();
  const [checkboxes, setCheckboxes] = useState({
    age: false,
    eula: false,
    privacy: false,
  });

  const allChecked = Object.values(checkboxes).every(value => value === true);

  const handleAccept = async () => {
    if (allChecked) {
      await updateAccount(true);
    }
  };

  const openUrl = (url: string) => {
    Linking.openURL(url);
  };

  return (
    <ScrollView style={styles.container}>
      <Text style={styles.title}>Terms and Conditions</Text>
      <Text style={styles.subtitle}>Please accept our terms and conditions to continue.</Text>

      <View style={styles.checkboxContainer}>
        <TouchableOpacity 
          style={styles.checkbox}
          onPress={() => setCheckboxes(prev => ({ ...prev, age: !prev.age }))}
        >
          <View style={[styles.checkboxBox, checkboxes.age && styles.checked]}>
            {checkboxes.age && <Text style={styles.checkmark}>✓</Text>}
          </View>
          <Text style={styles.checkboxLabel}>I confirm that I am 18 years or older</Text>
        </TouchableOpacity>

        <TouchableOpacity 
          style={styles.checkbox}
          onPress={() => setCheckboxes(prev => ({ ...prev, eula: !prev.eula }))}
        >
          <View style={[styles.checkboxBox, checkboxes.eula && styles.checked]}>
            {checkboxes.eula && <Text style={styles.checkmark}>✓</Text>}
          </View>
          <Text style={styles.checkboxLabel}>
            I accept the{' '}
            <Text 
              style={styles.link}
              onPress={() => openUrl('https://www.lifequest.website:8443/eula')}
            >
              End User License Agreement
            </Text>
          </Text>
        </TouchableOpacity>

        <TouchableOpacity 
          style={styles.checkbox}
          onPress={() => setCheckboxes(prev => ({ ...prev, privacy: !prev.privacy }))}
        >
          <View style={[styles.checkboxBox, checkboxes.privacy && styles.checked]}>
            {checkboxes.privacy && <Text style={styles.checkmark}>✓</Text>}
          </View>
          <Text style={styles.checkboxLabel}>
            I accept the{' '}
            <Text 
              style={styles.link}
              onPress={() => openUrl('https://www.lifequest.website:8443/privacy')}
            >
              Privacy Policy
            </Text>
          </Text>
        </TouchableOpacity>
      </View>

      <TouchableOpacity 
        style={[styles.acceptButton, !allChecked && styles.acceptButtonDisabled]}
        onPress={handleAccept}
        disabled={!allChecked}
      >
        <Text style={styles.acceptButtonText}>Accept</Text>
      </TouchableOpacity>
    </ScrollView>
  );
};

const styles = StyleSheet.create({
  container: {
    flex: 1,
    padding: 20,
    backgroundColor: '#fff',
  },
  title: {
    fontSize: 24,
    fontWeight: 'bold',
    marginBottom: 10,
    color: '#333',
  },
  subtitle: {
    fontSize: 16,
    marginBottom: 20,
    color: '#666',
  },
  checkboxContainer: {
    marginVertical: 20,
  },
  checkbox: {
    flexDirection: 'row',
    alignItems: 'center',
    marginBottom: 15,
  },
  checkboxBox: {
    width: 24,
    height: 24,
    borderWidth: 2,
    borderColor: '#007AFF',
    borderRadius: 4,
    marginRight: 10,
    justifyContent: 'center',
    alignItems: 'center',
  },
  checked: {
    backgroundColor: '#007AFF',
  },
  checkmark: {
    color: '#fff',
    fontSize: 16,
  },
  checkboxLabel: {
    flex: 1,
    fontSize: 16,
    color: '#333',
  },
  link: {
    color: '#007AFF',
    textDecorationLine: 'underline',
  },
  acceptButton: {
    backgroundColor: '#007AFF',
    padding: 15,
    borderRadius: 8,
    alignItems: 'center',
    marginTop: 20,
  },
  acceptButtonDisabled: {
    backgroundColor: '#cccccc',
  },
  acceptButtonText: {
    color: '#fff',
    fontSize: 18,
    fontWeight: 'bold',
  },
});

export default TermsAndConditionsComponent;